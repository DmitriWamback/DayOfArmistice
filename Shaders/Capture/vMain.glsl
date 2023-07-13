#version 410 core

layout(location=0) in vec3 fragp;
layout(location=1) in vec2 uv;

out VERTEX {
    vec3 fragp;
    vec2 uv;
} o;

void main() {
    o.fragp = fragp;
    o.uv = uv;
    gl_Position = vec4(fragp, 1.0);
}