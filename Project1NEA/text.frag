#version 330 core

in vec2 texCoord;

out vec4 outputColor;

uniform sampler2D fontAtlas;
uniform vec4 textColour;

void main()
{
    // The atlas stores white glyphs on a transparent background, so only the
    // alpha channel carries the glyph shape.
    float glyph = texture(fontAtlas, texCoord).a;
    outputColor = vec4(textColour.rgb, textColour.a * glyph);
}
