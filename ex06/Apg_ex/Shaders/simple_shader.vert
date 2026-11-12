#version 330 core
layout (location = 0) in vec3 aPosition;
layout (location = 2) in vec3 aNormal;
layout (location = 3) in vec4 aColor;
layout (location = 8) in vec2 aTex;

out vec3 fragPos;
out vec3 normal;
out vec3 color;
out vec2 uv;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

void main()
{
    uv = aTex;
    normal = aNormal * mat3(transpose(inverse(model)));
    fragPos = (vec4(aPosition, 1.0) * model).xyz;

    gl_Position = vec4(aPosition, 1.0) * model * view * projection;
}