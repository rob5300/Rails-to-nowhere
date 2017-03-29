using UnityEngine;
using System.Collections;


[ExecuteInEditMode]
public class RenderImage : MonoBehaviour {
    //Script was included with the brightness shader. Same source as .shader file.

	#region Variables
	public Shader curShader;
	public float brightnessAmount = 1.0f;
	public float saturationAmount = 1.0f;
	public float contrastAmount = 1.0f;
	private Material curMaterial;
	#endregion
	
	#region Properties
	Material material
	{
		get
		{
			if(curMaterial == null)
			{
				curMaterial = new Material(curShader);
				curMaterial.hideFlags = HideFlags.HideAndDontSave;	
			}
			return curMaterial;
		}
	}
	#endregion
	void Start () 
	{
		if(!SystemInfo.supportsImageEffects)
		{
			enabled = false;
			return;
		}
	}
	
	void OnRenderImage (RenderTexture sourceTexture, RenderTexture destTexture)
	{
		if(curShader != null)
		{
			material.SetFloat("_BrightnessAmount", brightnessAmount);
			material.SetFloat("_SaturationAmount", saturationAmount);
			material.SetFloat("_ContrastAmount", contrastAmount);
			Graphics.Blit(sourceTexture, destTexture, material);
		}
		else
		{
			Graphics.Blit(sourceTexture, destTexture);	
		}
		
		
	}
	
	void Update () 
	{
		brightnessAmount = Mathf.Clamp(brightnessAmount, 0.0f, 5f);
		saturationAmount = Mathf.Clamp(saturationAmount, 0.0f, 2.0f);
		contrastAmount = Mathf.Clamp(contrastAmount, 0.0f, 3.0f);
	}
	
	void OnDisable ()
	{
		if(curMaterial)
		{
			DestroyImmediate(curMaterial);	
		}
		
	}
	
}
