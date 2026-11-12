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
bool intersectSphere(vec3 O, vec3 d, vec3 C, float radius, out float tHit)
{
    // vector to center
    vec3 oc = O - C;
    
    // quadratic equation
    float a = dot(d, d);
    float b = 2.0 * dot(oc, d);
    float c = dot(oc, oc) - radius * radius;
    float discriminant = b * b - 4.0 * a * c;
    
    // solution exists
    if (discriminant > 0.0)
    {
        float t = (-b - sqrt(discriminant)) / (2.0 * a);
        if (t >= 0.0)
        {
            tHit = t;
            return true;
        }
    }
    return false;
}

// Newton Solver
//      a t^4 + b t^3 + c t^2 + d t + e = 0
//      tStart - starting value of t
//      tHit - value of parameter t at intersection 
//      returns - true if sphere present, false if not
bool solveTorusNewton(float a, float b, float c, float d, float e, float tStart, out float tHit)
{
    float t = tStart;
    int maxIt = 300;
    for (int i = 0; i < maxIt; i++)
    {
        float f = (((a * t + b) * t + c) * t + d) * t + e; // value
        float df = ((4.0 * a * t + 3.0 * b) * t + 2.0 * c) * t + d; // derivative
        if (abs(df) < 1e-6) break;

        float tNext = t - f / df; // next value
        if (abs(tNext - t) < 1e-4) // found intersection
        {
            if (tNext >= 0.0)
            {
                tHit = tNext;
                return true;
            }
        }
        t = tNext;
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
bool intersectTorus(vec3 O, vec3 d, vec3 C, float radius1, float radius2, mat4 rotMatrix, out float tHit)
{
    // Bounding sphere pre-test discards misses & finds ideal starting point for Newton solver
    float boundingRadius = radius1 + radius2;
    float tNear;
    if (!intersectSphere(O, d, C, boundingRadius, tNear))
    {
        return false;
    }

    // Transform origin and ray target into torus space and rotate them
    vec3 transl = vec3(0.0, 0.0, -C.z);
    vec4 O_T = vec4(O + transl, 1.0) * rotMatrix;
    vec4 R_T = vec4((O + d) + transl, 1.0) * rotMatrix;

    vec3 O_rot = O_T.xyz;
    vec3 d_rot = R_T.xyz - O_rot;

    float dDot = dot(d_rot, d_rot);
    float Od = dot(O_rot, d_rot);

    float r1Sq = radius1 * radius1;
    float r2Sq = radius2 * radius2;
    float oDotO = dot(O_rot, O_rot);

    // Coefficients derived from implicit torus equation
    float a = dDot * dDot;
    float b = 4.0 * dDot * Od;
    float c = 2.0 * dDot * (oDotO - (r2Sq + r1Sq)) + 4.0 * Od * Od + 4.0 * r2Sq * d_rot.z * d_rot.z;
    float dCoeff = 4.0 * Od * (oDotO - (r2Sq + r1Sq)) + 8.0 * r2Sq * O_rot.z * d_rot.z;
    float ep = (oDotO - (r2Sq + r1Sq));
    float e = ep * ep + 4.0 * r2Sq * (O_rot.z * O_rot.z - r1Sq);

    return solveTorusNewton(a, b, c, dCoeff, e, tNear, tHit);
}

void main()
{
    int u_drawMode = 0;
    // Scale factor to keep float arithmetic within precision limits (~1.0)
    const float scale = 0.001;

    vec2 screenPos = vec2(TexCoord.x - 0.5, 0.5 - TexCoord.y) * (resolution * scale);
    vec3 rayPoint = vec3(screenPos.x, screenPos.y, 0.0);

    vec3 O = vec3(0.0, 0.0, -1000.0 * scale);
    vec3 d = rayPoint - O;

    vec4 color = vec4(0.165, 0.165, 0.165, 1.0);

    if (u_drawMode == 0)
    {
        // Sphere 3D
        vec3 C = vec3(0.0, 0.0, 200.0 * scale);
        float radius = 199.0 * scale;
        float t;
        if (intersectSphere(O, d, C, radius, t))
        {
            // Distance from camera origin O to front of sphere
            float tMin = length(C - O) - radius;
            // Normalized depth: 0.0 at front center, 1.0 at silhouette depth
            float normDepth = clamp((t - tMin) / radius, 0.0, 1.0);
            // Invert so center is 1.0 and edge is 0.0
            float centerWeight = 1.0 - normDepth;
            float cVal = pow(centerWeight, 2.5);
            color = vec4(cVal, cVal, cVal, 1.0);
        }
    }
    else if (u_drawMode == 1)
    {
        // Torus 3D
        vec3 C = vec3(0.0, 0.0, 800.0 * scale);
        float radius1 = 120.0 * scale;
        float radius2 = 220.0 * scale;
        float t;
        if (intersectTorus(O, d, C, radius1, radius2, rot, t))
        {
            float tMin = length(C - O) - (radius2+radius1);
            float normDepth = clamp((t - tMin) / (radius2+radius1), 0.0, 1.0);
            float centerWeight = 1.0 - normDepth;
            float cVal = pow(centerWeight, 2.5);
            color = vec4(cVal, cVal, cVal, 1.0);
        }
    }

    FragColor = color;
}