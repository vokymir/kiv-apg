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

void main()
{
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
    vec3 reflectDir = - reflect(lightDir, norm);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), 32);
    vec3 specCol = spec * lightCol;

    // rim
    float rimFac = max(1.0 - dot(norm, viewDir), 0.0);
    rimFac = pow(rimFac, 2);
    vec3 rimCol = vec3(1.0, 0.0, 0.0) * rimFac;

    // outline
    float outline = abs(dot(norm, viewDir));
    // step(value, edge) => 0.0 if x<edge, 1.0 if x>edge 
    float isInside = 1.0; // step(0.3, outline);

    vec3 lightResult = (ambCol + diffCol + specCol + rimCol) * tex1.xyz;
    // mix(x,y,a) => x(1-a) + ya
    vec3 result = mix(vec3(0.0), lightResult, isInside);
    FragColor = vec4(result, 1.0);
}