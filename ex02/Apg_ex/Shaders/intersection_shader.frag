#version 330 core

in vec2 TexCoord;
out vec4 FragColor;

uniform vec2 resolution; // Canvas width and height
uniform mat4 rot;        // Camera rotation matrix

// Ray-Sphere Intersection
//      O - viewer
//      d - direction of view
//      C - center of sphere
//      radius - sphere radius
//      tHit - value of parameter t at intersection
//      returns - true if sphere present, false if not
bool intersectSphere(vec3 O, vec3 d, vec3 C, float radius, out float tHit) {
  // TODO
  float a = dot(d, d);
  vec3 o_min_c = O - C;
  float b = 2 * dot(o_min_c, d);
  float c = dot(o_min_c, o_min_c) - radius * radius;
  float discriminant = b * b - 4 * a * c;

  if (discriminant < 0) {
    return false;
  }

  float t1 = (-b - sqrt(discriminant)) / (2 * a);
  float t2 = (-b + sqrt(discriminant)) / (2 * a);
  if (t1 > 0 && t2 > 0) {
    tHit = t1 < t2 ? t1 : t2;
  } else if (t1 > 0) {
    tHit = t1;
  } else if (t2 > 0) {
    tHit = t2;
  }
  return true;
}

// Newton Solver
//      a t^4 + b t^3 + c t^2 + d t + e = 0
//      tStart - starting value of t
//      tHit - value of parameter t at intersection
//      returns - true if sphere present, false if not
bool solveTorusNewton(float a, float b, float c, float d, float e, float tStart,
                      out float tHit) {
  float t = tStart;
  int maxIt = 300;

  // TODO newton solver
  float epsilon = 0.0001;
  float ft = 0;
  float ft_der = 0;
  for (int i = 0; i < maxIt; i++) {
    ft = a * t * t * t * t + b * t * t * t + c * t * t + d * t + e;
    ft_der = 4 * a * t * t * t + 3 * b * t * t + 2 * c * t + d;

    if (ft < epsilon && -ft < epsilon) {
      tHit = t;
      return true;
    }
    t = t - ft / ft_der;
  }

  return false;
}

// Ray-Torus Intersection
//      O - viewer
//      d - direction of view
//      C - center of sphere
//      radius1 - inner radius of torus tube
//      radius2 - radius of torus
//      rotMatrix - current rotation of torus
//      tHit - value of parameter t at intersection
//      returns - true if torus present, false if not
bool intersectTorus(vec3 O, vec3 d, vec3 C, float radius1, float radius2,
                    mat4 rotMatrix, out float tHit) {
  // TODO test torus

  // Bounding sphere pre-test
  // - test circumsphere of the torus
  //      - if it hits, it makes sense to test the torus
  //      - if it does not hit, do not test the torus
  // - this test also finds ideal starting point for the Newton solver
  float tNear = 0;

  if (!intersectSphere(O, d, C, 2 * (radius2 + radius1), tNear)) {
    return false;
  }

  // Apply rotation
  vec4 Ohomo = vec4(O, 1);
  vec4 dhomo = vec4(d, 0);
  Ohomo = Ohomo * rotMatrix;
  dhomo = dhomo * rotMatrix;
  O = vec3(Ohomo);
  d = vec3(dhomo);

  // Coefficients derived from implicit torus equation
  float a = dot(d, d) * dot(d, d);
  float b = 4 * dot(d, d) * dot(O, d);
  float c =
      2 * dot(d, d) * (dot(O, O) - (radius2 * radius2 + radius1 * radius1)) +
      4 * dot(O, d) * dot(O, d) +
      4 * radius2 * radius2 * d.z * d.z; // maybe d[2]
  float dCoeff =
      4 * dot(O, d) * (dot(O, O) - (radius2 * radius2 + radius1 * radius1)) +
      8 * radius2 * radius2 * O.z * d.z;
  float e = (dot(O, O) - (radius2 * radius2 + radius1 * radius1)) *
                (dot(O, O) - (radius2 * radius2 + radius1 * radius1)) +
            4 * radius2 * radius2 * (O.z * O.z - radius1 * radius1);

  return solveTorusNewton(a, b, c, dCoeff, e, tNear, tHit);
}

void main() {
  int u_drawMode = 1;
  // Scale factor to keep float arithmetic within precision limits (~1.0)
  const float scale = 0.001;

  // Position of fragment we are looking at
  vec2 screenPos =
      vec2(TexCoord.x - 0.5, 0.5 - TexCoord.y) * (resolution * scale);
  vec3 rayPoint = vec3(screenPos.x, screenPos.y, 0.0);

  // Position of viewer
  vec3 O = vec3(0.0, 0.0, -1000.0 * scale);

  // Ray direction
  vec3 d = rayPoint - O;

  // Background color
  vec4 color = vec4(0.165, 0.165, 0.165, 1.0);

  // Compute the object
  if (u_drawMode == 0) {
    // Sphere 3D
    vec3 C = vec3(0.0, 0.0, 200.0 * scale);
    float radius = 199.0 * scale;
    float t;
    if (intersectSphere(O, d, C, radius, t)) {
      color = vec4(1 / t / (1300 * scale));
    }
  } else if (u_drawMode == 1) {
    // Torus 3D
    vec3 C = vec3(0.0, 0.0, 800.0 * scale);
    float radius1 = 120.0 * scale;
    float radius2 = 220.0 * scale;
    float t;
    if (intersectTorus(O, d, C, radius1, radius2, rot, t)) {
      color = vec4(t / (1300 * scale));
      // color = vec4(1.0);
    }
  }

  FragColor = color;
}