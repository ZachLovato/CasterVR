
void FuncTest (out Vector4 OUT){

OUT = IN;
float4 scaleOffset = unity_StereoScaleOffset[unity_StereoEyeIndex];
 
	//check that stereo is enabled
	if (scaleOffset.x > 0){
 
		OUT.x /= 2;
		OUT.x += scaleOffset.z;
	}
}