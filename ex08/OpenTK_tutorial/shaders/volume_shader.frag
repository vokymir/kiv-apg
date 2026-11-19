#version 330 core
out vec4 FragColor;

in vec2 uv;

uniform sampler2D texture0;
uniform sampler2D texture1;
uniform sampler3D texture2;

uniform float t;

vec4 scene(vec3 pos)
{
    vec4 color;

    float scale = 20;
    pos *= scale;
    color.xyzw = vec4(step(0.7,dot(vec3(1,1,1),sin(pos))));
    pos *= 2;
    vec3 norm = -cos(pos);
    color.xyz *= 0.3 + 0.4 * clamp(dot(vec3(0.5,0.5,0.5), normalize(norm)), 0, 1);
    return color;
}

vec3 computeFromFunction(vec4 from, vec4 dir) {
    vec3 pos;
    vec3 color = vec3(0.0);
    float alpha = 0;

    // TODO
    // do 100 steps
        // compute position at which we will sample
        // compute color
        // if opacity is too low, then skip, threshold = 0.05
        // blend color and alpha
        // if alpha is more than 1, end early

    return color;
}

// compute from 3D texture
vec3 computeFromTexture(vec4 from, vec4 dir) {
    vec3 pos;
    vec3 color = vec3(0.0);
    float alpha = 0;

    // texture is 256x256x109
    // step size should come from width of one voxel, but here voxels are not homogenous
    float stepSize = 1 / (256*2.0); // 2 samples per voxel along any axis 

    // TODO
    // iteratively sample
        // compute position at which we will sample
        // sample color from texture
        // if opacity is too low, then skip, threshold = 0.05
        // opacity correction to original 0.01 step size
        // blend color and alpha
        // if alpha is more than 1, end early

    return color;
}

void main()
{
    vec4 from = texture(texture0, uv);
    vec4 to = texture(texture1, uv);
    vec4 dir = (to-from);

    // TODO
    // if dir vector too short - stop rendering early

    // compute color
    vec3 color = computeFromFunction(from, dir);
    // vec3 color = computeFromTexture(from, dir);

    FragColor = vec4(color, 1);
}