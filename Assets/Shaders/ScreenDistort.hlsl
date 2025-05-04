
//UNITY_SHADER_NO_UPGRADE
#ifndef MYHLSLINCLUDE_INCLUDED
#define MYHLSLINCLUDE_INCLUDED

void screenDistort_float(float2 uv, float bend, float edge, out float2 distortedUv)
{
    	// put in symmetrical coords
	uv = (uv - 0.5) * 2.0;

	uv *= 1.0 + edge;	

	// deform coords
	uv.x *= 1.0 + pow((abs(uv.y) / bend), 2.0);
	uv.y *= 1.0 + pow((abs(uv.x) / bend), 2.0);

	// transform back to 0.0 - 1.0 space
	uv  = (uv / 2.0) + 0.5;

	distortedUv = uv;
}
#endif //MYHLSLINCLUDE_INCLUDED