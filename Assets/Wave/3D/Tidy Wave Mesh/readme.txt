Hello friend, and congratulations on your purchase of Tidy Wave Mesh!

This is a handy little script that will give you a vaguely-liquid look via realtime mesh manipulation.

To use this script:

1) Pick a mesh to use. Tidy Wave Mesh will work on any mesh, but in general you want a mesh with a lot of vertices along the top (y-axis). The Unity default Plane mesh is a good start.
2) Drag your mesh object into the scene.
3) Drag the WaveMesh.cs script onto the object.
4) (Optional) Drag your target Mesh Filter onto the MeshFilter field on the WaveMesh script. This is not mandatory - the script will automatically search for a Mesh Filter at runtime. Only use this if you have some sort of odd MeshFilter target desires.
5) Click on the "Wave Curve" field on the inspector for the WaveMesh script on your object.
6) Set your wave curve here as desired. Generally you will want to set the repeat mode to "Ping-pong" by clicking on the end-point of your curve and selecting from the drop-down.
7) Press play!
8) If you see odd results, you may need to tweak the "Vertex Inclusion Margin" - this is the margin that decides whether to include a vertex in the list of vertices to modify. A higher value will include more (and lower) vertices in the list, a smaller value will include less. 
9) Play around with your curves - you can get a lot of interesting variations through this.
10) Enjoy!

If you need assistance, or have a request - feel free to contact us (me) at support@dopplerinteractive.com

Have fun!


Your friend,
Joshua McGrath