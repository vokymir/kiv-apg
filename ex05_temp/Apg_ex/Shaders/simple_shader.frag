#version 330 core
out vec4 FragColor;

in vec3 fragPos;
in vec3 normal;
in vec3 color;
in vec2 uv;

uniform sampler2D texture0;

uniform vec3 lightCol;
uniform vec3 lightPos;
uniform vec3 viewPos;

int toonLVL = 2;
float toonScale = 1.0 / toonLVL;

void main() {
  vec4 tex1 = texture(texture0, uv);
  vec3 norm = normalize(normal);

  // ambient
  float ambStr = 0.3;
  vec3 ambCol = ambStr * lightCol;

  // diffuse
  vec3 lightDir = normalize(lightPos - fragPos);
  float diff = max(dot(lightDir, norm), 0.0);
  // diff = ceil(diff * toonLVL) * toonScale;
  vec3 diffCol = diff * lightCol;

  // specular
  vec3 viewDir = normalize(viewPos - fragPos);
  vec3 reflectDir = -reflect(lightDir, norm);
  float spec = pow(max(dot(viewDir, reflectDir), 0.0), 32);
  vec3 specCol = spec * lightCol;

  // rim
  float rimFac = max(1.0 - dot(norm, viewDir), 0.0);
  rimFac = pow(rimFac, 2);
  vec3 rimCol = vec3(1.0, 0.0, 0.0) * rimFac;

  // TODO outline
  // - if fragment is "perpendicular enough" to viewer
  // - instead of if statements => step(x, edge) & mix(edgeColor, lightResult,
  // condition)
  //          step(x, edge) => 0.0 if x<edge, 1.0 if x>edge
  //          mix(x,y,a) => x(1-a) + ya
  // float edgeVal = abs(dot(norm, viewDir));
  // float isEdge = step(edgeVal, 0.5);
  // vec3 outlineCol = vec3(0.0, 1.0, 0.0);

  vec3 lightResult = (ambCol + diffCol + specCol + rimCol) * tex1.xyz;

  // vec3 res = mix(lightResult, outlineCol, isEdge);
  // FragColor = vec4(res, 1.0);
  FragColor = vec4(lightResult, 1.0);
}