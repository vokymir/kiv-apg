#version 330 core
in vec2 uv;
out vec4 FragColor;

uniform sampler2D uColorTex; // color texture
uniform sampler2D uDepthTex; // depth texture
uniform vec4 uBgColor; // background color
uniform vec2 uTexelSize; // size of texel

// 8 masks in 1D array
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
// isForeground - sampled if neighborhood is background or foreground
bool testMask(int maskIdx, int isForeground[9]) {
    // TODO
    // test if the neighborhood fits the mask (bg is bg, object is object)

    return true;
}

// Test if the fragment is on the outer silhouette of the object
bool isSilhouette(int isForeground[9]) {
    // TODO
    // if any mask fits - it is silhouette, return true
    // else return false
    
    return false;
}

void main() {
    // current color and depth
    vec4 currentColor = texture(uColorTex, uv);
    vec4 currentDepth = texture(uDepthTex, uv);

    // only process background fragments
    // if the fragment is not background, then quit early
    if (distance(currentColor, uBgColor) >= 0.001) {
        FragColor = currentColor;
        gl_FragDepth = currentDepth.r;
        return;
    }

    int isForeground[9];
    float depth[9];
    vec4 colors[9];
    int idx = 0;

    // TODO
    // check 3x3 neighborhood
    // - keep depth and color 
    // - classify each fragment from neighborhood as background or foreground
    

    // test if the fragment should be replaced - is it part of the "outer" sillhouette?
    if (!isSilhouette(isForeground)) {
        // if the fragment should be replaced
        // find minimum depth from neighborhood (the neighbor closest to screen)
        float minDepth = 1.0;
        int minIndex = 4;

        // TODO

        // pass color and depth
        FragColor = colors[minIndex];
        gl_FragDepth = minDepth;
        
        // TO DEBUG - try setting to some noticeable color
        // FragColor = vec4(1.0, 1.0, 1.0, 1.0);

    } else {
        // pass color and depth
        FragColor = currentColor;
        gl_FragDepth = currentDepth.r;
    }
}