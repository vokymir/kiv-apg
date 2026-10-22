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

int reflection_strength = 32;
int rim_factor = 4;
vec3 rim_col = vec3(0, 1, 0);

int toonLVL = 2;
float toonScale = 1.0 / toonLVL;

void main() {
  vec4 tex1 = texture(texture0, uv);
  vec3 norm = normalize(normal);
  vec3 v_pos = normalize(viewPos);

  // TODO
  // ambient
  vec3 ambCol = vec3(0.1);

  // diffuse
  vec3 light_dir = normalize(lightPos - fragPos);
  float cos_alpha = dot(light_dir, norm);
  cos_alpha = max(cos_alpha, 0);

  cos_alpha = int(cos_alpha * toonLVL) * toonScale;

  vec3 diffCol = vec3(cos_alpha) * lightCol;

  // specular
  vec3 reflected = reflect(-1.0f * light_dir, norm);
  float spec = dot(reflected, v_pos);
  spec = clamp(spec, 0, 1);
  spec = pow(spec, reflection_strength);
  vec3 specCol = vec3(spec) * lightCol;

  // rim
  vec3 view_dir = normalize(viewPos - fragPos);
  cos_alpha = 1 - clamp(dot(view_dir, norm), 0, 1);
  cos_alpha = pow(cos_alpha, rim_factor);

  vec3 rimCol = vec3(cos_alpha) * rim_col;

  vec3 result = (ambCol + diffCol + specCol + rimCol) * (tex1.xyz);
  FragColor = vec4(result, 1.0);
}