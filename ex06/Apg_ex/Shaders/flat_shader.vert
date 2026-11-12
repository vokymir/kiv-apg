#version 330 core
layout (location = 0) in vec3 aPosition;
layout (location = 2) in vec3 aNormal;

out vec3 normal;
out vec3 color;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;
uniform vec3 baseColor;

void main()
{
    normal = aNormal;
    color = baseColor;
    gl_Position = vec4(aPosition, 1.0) * model * view * projection;
}