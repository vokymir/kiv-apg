#version 330 core
out vec4 FragColor;

in vec3 fragPos;
in vec3 normal;
in vec3 color;
in vec2 uv;

uniform sampler2D texture0;
uniform sampler2D texture1;

uniform float isTex;
uniform vec3 viewPos;

void main()
{
    vec4 tex1 = mix(vec4(color, 1.0), texture(texture0, uv), isTex);

    vec3 norm = normalize(normal);
    vec3 viewDir = normalize(viewPos - fragPos);

    /* if there are normals i can try to filter
    float angle = dot(norm, viewDir);
    float isAngled = step(angle, 0);
    tex1 = mix(tex1, vec4(0, 0, 0, 0), isAngled);
    */

    FragColor = tex1;
}