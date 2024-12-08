#ifndef COLOR_HELPER_INCLUDED
#define COLOR_HELPER_INCLUDED

#ifndef PI
#define PI 3.141592653589793
#endif

#ifndef HALF_PI
#define HALF_PI 1.5707963267948966
#endif

// Helper Funtions

inline float clampValue(float input, float2 limit) 
{
	float minValue = 1-limit.y;
	float maxValue = 1-limit.x;
	if(input<=minValue){
		return 0;
	} else if(input>=maxValue){
		return 1;
	} else {
		return (input - minValue )/(maxValue-minValue);
	}						
}

inline float2 rotateUV(fixed2 uv, float rotation) 
{
	float sinX = sin (rotation);
	float cosX = cos (rotation);
	float2x2 rotationMatrix = float2x2( cosX, -sinX, sinX, cosX);
	return mul ( uv, rotationMatrix )/2 + 0.5;
}

inline fixed3 lerp3(fixed3 a, fixed3 b, fixed3 c, float pos, float size){

	float ratio2 = 0.5+size*0.5;
	float ratio1 = 1-ratio2;

	if(pos<ratio1)
		return lerp(a,b,pos/ratio1);
	else if(pos>ratio2)
		return lerp(b,c,(pos-ratio2)/ratio1);				
	else
		return b;
}


#endif