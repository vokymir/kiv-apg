#version 330 core
in vec2 uv;
out vec4 FragColor;

uniform sampler2D uColorTex; // color texture
uniform sampler2D uDepthTex; // depth texture
uniform vec4 uBgColor; // background color
uniform float uMinD; // minimum distane to detect discontinuity
uniform vec2 uTexelSize; // size of texel

// 8 structural masks in 1D array
const int masks[72] = int[72](
    1, 0, 0,  1, 0, 0,  1, 0, 0, // mask 0
    0, 0, 0,  0, 0, 0,  1, 1, 1, // mask 1
    0, 0, 1,  0, 0, 1,  0, 0, 1, // mask 2
    1, 1, 1,  0, 0, 0,  0, 0, 0, // mask 3
    0, 0, 0,  1, 0, 0,  1, 1, 0, // mask 4
    0, 0, 0,  0, 0, 1,  0, 1, 1, // mask 5
    0, 1, 1,  0, 0, 1,  0, 0, 0, // mask 6
    1, 1, 0,  1, 0, 0,  0, 0, 0  // mask 7
);

// Test one mask
// maskIdx - index of the mask
// depth - sampled depth
// centerDepth - depth at center of the samples
// minD - minimum distane to detect discontinuity
bool testMask(int maskIdx, float depth[9], float centerDepth, float minD) {
    // TODO
    // test if the mask fits = bg is bg, object is object
    // AND if the depth of object is not further from center depth than minD
    
    return true;
}

// Test if the fragment should be covered or not
// depth - sampled depth
// minD - minimum distane to detect discontinuity
bool isLegit(float depth[9], float minD) {
    // TODO
    // if no mask fits return false - this is not a legit fragment, it will need to be covered up
    
    return false;
}

void main() {
    // get color of fragment
    vec4 currentColor = texture(uColorTex, uv);
    
    // pixel is background -> ignore
    if (distance(currentColor, uBgColor) < 0.001) {
        FragColor = currentColor;
        return;
    }

    float depth[9];
    vec4 colors[9];
    int idx = 0;
    
    // TODO
    // in 3x3 neighborhood sample color and depth

    // test if fragment is legit
    if (!isLegit(depth, uMinD)) {
        // if it needs to be covered 
        // find minimum depth from neighborhood (the neighbor closest to screen)
        float minDepth = 1.0;
        int minIndex = 4;
        
        // TODO

        // set frag color to that color
        FragColor = colors[minIndex];

        // TO DEBUG - try setting to some noticeable color
        // FragColor = vec4(1.0, 1.0, 1.0, 1.0);
    } else {
        FragColor = currentColor;
    }
}