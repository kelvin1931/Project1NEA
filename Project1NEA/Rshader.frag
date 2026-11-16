#version 330 core

out vec4 outputColor;

in vec2 texCoord;
in vec3 fragPos;
in vec3 normal;

uniform sampler2D texture1;
uniform vec3 lightPos;
uniform vec3 lightColor;
uniform int unlit;

void main()
{
    vec4 tex = texture(texture1, texCoord);
if (unlit == 1)
    {
        outputColor = tex;
        return;
    }
    float ambient = 0.12;
    vec3 norm = normalize(normal);
    vec3 lightDir = normalize(lightPos - fragPos);
    float diff = max(dot(norm,lightDir) , 0.0);
    outputColor =   vec4((ambient + diff) * lightColor * tex.rgb , tex.a);
}