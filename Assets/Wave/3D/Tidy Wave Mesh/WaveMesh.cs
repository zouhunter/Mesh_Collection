using System.Collections.Generic;
using UnityEngine;

public class WaveMesh : MonoBehaviour
{
	int[] vertexIndices;
	
	public MeshFilter meshFilter;
	//high point will serve as the base middle-point for rippling
	float highPoint = 0.0f;
	float leftMost = float.MaxValue;
	float rightMost = 0.0f;
	//point span will assist us with normalizing x across the width of the mesh
	float pointSpan = 0.0f;
	
	//if the points are within this amount of each other on the mesh, they will be counted as being "on the same level"
	public float vertexInclusionMargin = 0.01f;
    [Range(0.01f,100)]
    public float speed = 1f;
	public AnimationCurve waveCurve;
	
	public bool recalculateNormals = true;
	
	bool valid = true;
	
	void Start(){
		
		if(meshFilter == null){
			
			meshFilter = gameObject.GetComponent<MeshFilter>();
			
		}
		
		if(meshFilter == null){
			
			meshFilter = gameObject.GetComponentInChildren<MeshFilter>();
			
		}
		
		if(meshFilter == null){
			
			Debug.LogWarning("You must assign a MeshFilter to the WaveMesh script attached to object: " + name);
			
			valid = false;
			
			return;
			
		}
		
		//we look at the mesh, and take all of the highest vertices (on the y axis)
		//these are the verts we wish to manipulate
		Mesh m = meshFilter.mesh;
		
		//First, we establish the high point
		//and the right and left most points on the x axis
		//this will assist us with ripples and such
		highPoint = 0.0f;
		leftMost = float.MaxValue;
		rightMost = 0.0f;
		
		for(int i= 0; i < m.vertices.Length; i++){
			
			if(m.vertices[i].y > highPoint){
				
				//highPoint = Mathf.Round(m.vertices[i].y);
				
				highPoint = m.vertices[i].y;
				
			}
			
			if(m.vertices[i].x > rightMost){
				rightMost = m.vertices[i].x;
			}
			
			if(m.vertices[i].x < leftMost){
				leftMost = m.vertices[i].x;
			}
		}
		
		pointSpan = rightMost - leftMost;
		
		//We move through the array, and find all of the highest vertices
		List<int> highVertexIndices = new List<int>();
		
		for(int i= 0; i < m.vertices.Length; i++){
			
			//if(Mathf.Round(m.vertices[i].y) == highPoint){
			if(Mathf.Abs(m.vertices[i].y - highPoint) <= vertexInclusionMargin){
				highVertexIndices.Add(i);
			}
		}
		
		//now we know the indices of these vertices, which is just great
		vertexIndices = highVertexIndices.ToArray();
		
	}
	
	float sineCounter = 0.0f;
	
	//During update, we'll simply iterate through the vertices we've counted as being
	//the 'highest' vertices (those at the highest y point, with a difference of vertexInclusionMargin)
	//For each of these, we'll take their normalized x (their position between 0 and 1 on the x axis of the mesh)
	//and set the height of this vertex to correspond to the point in the animation curve.
	//Beautiful.
	void Update(){
		
		//Why do we use this boolean?
		//I was going to check if(meshFilter == null) on every update
		//but I believe (believe) that a comparison of objects to null 
		//is slightly more expensive than a comparison of booleans
		//reality or paranoia?
		//you decide!
		if(!valid){
			return;
		}
		
		sineCounter += Time.deltaTime * 5;
		
		Vector3[] verts = meshFilter.mesh.vertices;
		
		for(int i = 0; i < vertexIndices.Length; i++){
			
			Vector3 vert = verts[vertexIndices[i]];
			
			vert.y = highPoint + GetCurveHeight(GetNormalizedVertexX(vert),Time.time * speed);
			
			verts[vertexIndices[i]] = vert;
		}
		
		meshFilter.mesh.vertices = verts;
		
		if(recalculateNormals){
			meshFilter.mesh.RecalculateNormals();
		}
		
	}
	
	float GetCurveHeight(float normalized_x, float time){
		
		return waveCurve.Evaluate(normalized_x + time);
		
	}
		
	float GetNormalizedVertexX(Vector3 vertex){
		
		float n =((vertex.x + Mathf.Abs(leftMost)) / pointSpan) * 2.0f -1.0f;
		
		return n; 		
	}
}

