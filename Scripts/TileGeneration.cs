using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Tilemaps;

namespace Nandan{
	public class TileGeneration : MonoBehaviour
	{
		private Camera cam;
		private int height;
		private int width;
		private List<Room> rooms;
		[SerializeField] private RuleTile wallTile;
		[SerializeField] private Tile floorTile;
		[SerializeField] private Tile fillTile;
		[SerializeField] private Tilemap wallMap;
		[SerializeField] private Tilemap floorMap;
		private Vector3Int spawnPoint;
		[SerializeField] private Transform player;
		[SerializeField] private NavMeshSurface2d navmesh;
		[SerializeField] private GameObject enemy;

		public struct Room{
			public int roomWidth;
			public int roomHeight;
			public Vector3Int startCoord;

			public void SetStartCoord(Vector3Int coord){
				startCoord = coord;
			}
		}

		void Start()
		{
			rooms = new List<Room>();
			CalculateThroneRoom();
		}

		private void Update()
		{
			if(Input.GetKeyDown(KeyCode.R)){
				//Retry();
			}
		}

		public void CalculateThroneRoom(){
			height = 45;
			width = 80;
			int roomWidth = Random.Range(14, 16);
			int roomHeight = Random.Range(8, 20);
			Vector3Int startCoord = new Vector3Int(Random.Range(0, width - roomWidth), Random.Range(0, height - roomHeight), 0);

			Room room = new Room();
			room.roomWidth = roomWidth;
			room.roomHeight = roomHeight;
			room.startCoord = startCoord;
			rooms.Add(room);

			GenerateRoom(roomWidth, roomHeight, startCoord);
			spawnPoint = new Vector3Int(startCoord.x+roomWidth/2, startCoord.y+roomHeight/2,0);
			player.position = spawnPoint;

			for (int i = 0; i < 9; i++) {
				CalculateDimensions();
			}

			ConnectPassageways();
			FillRoom();
			navmesh.BuildNavMesh();
			SpawnEnemies();
		}

		public void CalculateDimensions(){
			height = 45;
			width = 80;
			int roomWidth = Random.Range(8, 12);
			int roomHeight = Random.Range(8, 12);

			Vector3Int startCoord = new Vector3Int(Random.Range(0, width - roomWidth), Random.Range(0, height - roomHeight), 0);
			int attempt = 0;

			Room room = new Room();
			room.roomWidth = roomWidth;
			room.roomHeight = roomHeight;
			room.startCoord = startCoord;

			while (!Validate(room)){
				startCoord = new Vector3Int(Random.Range(0, width - roomWidth), Random.Range(0, height - roomHeight), 0);
				room.startCoord = startCoord;
				attempt++;
				if(attempt == 49){
					return;
				}
			}

			rooms.Add(room);
			GenerateRoom(roomWidth, roomHeight, startCoord);

		}

		private bool Validate(Room room){

			Vector3Int coord = new Vector3Int(room.startCoord.x - 1, room.startCoord.y - 1, 0);

			for (int i = 0; i < room.roomWidth+2; i++)
			{
				for (int j = 0; j < room.roomHeight+2; j++)
				{
					if(floorMap.HasTile(coord) || wallMap.HasTile(coord)){
						return false;
					}

					coord = new Vector3Int(coord.x, coord.y + 1, 0);
				}
				coord = new Vector3Int(room.startCoord.x + i, room.startCoord.y - 1, 0);
			}

			return true;
		}


		private void GenerateRoom(float roomWidth, float roomHeight, Vector3Int startCoord){

			Vector3Int coord = startCoord;

			for(int i = 0; i < roomWidth; i++){
				for(int j = 0; j < roomHeight; j++){
					if(i == 0 || i == roomWidth-1 || j == 0 || j == roomHeight-1){
						if (!floorMap.HasTile(coord)){
							wallMap.SetTile(coord, wallTile);
						}
					} else{
						if(wallMap.HasTile(coord)){
							wallMap.SetTile(coord, null);
						}
						floorMap.SetTile(coord, floorTile);
					}
					coord = new Vector3Int(coord.x, coord.y+1, 0);
				}
				coord = new Vector3Int(startCoord.x + i + 1, startCoord.y, 0);
			}

		}

		public void ConnectPassageways(){

			Shuffle(rooms);

			for(int i = 0; i < rooms.Count-1; i++){
				Room firstRoom = rooms[i];
				Room secondRoom = rooms[i + 1];
				Vector3Int firstMid = new Vector3Int(firstRoom.startCoord.x + firstRoom.roomWidth / 2, firstRoom.startCoord.y + firstRoom.roomHeight / 2, 0);
				Vector3Int secondMid = new Vector3Int(secondRoom.startCoord.x + secondRoom.roomWidth / 2, secondRoom.startCoord.y + secondRoom.roomHeight / 2, 0);
				int horizontalPassageWidth = Mathf.Abs(firstMid.x - secondMid.x) + 1;
				int verticalPassageHeight = Mathf.Abs(firstMid.y - secondMid.y) + 1;

				if(firstMid.x < secondMid.x){
					//construct right passageway
					Vector3Int startCoord = new Vector3Int(firstMid.x, firstMid.y - 1, 0);
					GenerateRoom(horizontalPassageWidth, 3, startCoord);
					if (firstMid.y < secondMid.y)
					{
						//construct up passageway
						startCoord = new Vector3Int(firstMid.x+horizontalPassageWidth-2, firstMid.y - 1, 0);
						GenerateRoom(3, verticalPassageHeight, startCoord);
					}
					else if (firstMid.y > secondMid.y)
					{
						//construct down passageway
						startCoord = new Vector3Int(secondMid.x - 1, secondMid.y+1, 0);
						GenerateRoom(3, verticalPassageHeight, startCoord);
					}
				} else if(firstMid.x > secondMid.x){
					//construct left passageway
					Vector3Int startCoord = new Vector3Int(firstMid.x-horizontalPassageWidth, firstMid.y - 1, 0);
					GenerateRoom(horizontalPassageWidth, 3, startCoord);
					if (firstMid.y < secondMid.y)
					{
						//construct up passageway
						startCoord = new Vector3Int(firstMid.x - horizontalPassageWidth, firstMid.y - 1, 0);
						GenerateRoom(3, verticalPassageHeight, startCoord);
					}
					else if (firstMid.y > secondMid.y)
					{
						//construct down passageway
						startCoord = new Vector3Int(secondMid.x - 1, secondMid.y+1, 0);
						GenerateRoom(3, verticalPassageHeight, startCoord);
					}
				}

			}

		}

		public void FillRoom()
		{
			Vector3Int coord;

			for (int i = -10; i < 95; i++)
			{
				for (int j = -10; j < 55; j++)
				{
					coord = new Vector3Int(i, j, 0);
					if (!floorMap.HasTile(coord) && !wallMap.HasTile(coord)){
						wallMap.SetTile(coord, fillTile);
					}
				}
			}
		}

		public void SpawnEnemies(){
			Quaternion rotation = new Quaternion();
			rotation.eulerAngles = Vector3.zero;
			foreach(Room room in rooms){
				Instantiate(enemy, new Vector3Int(room.startCoord.x + Random.Range(1, room.roomWidth - 1), room.startCoord.y + Random.Range(1, room.roomHeight - 1), 0), rotation);
			}
		}

		/*public void Retry(){
			wallMap.ClearAllTiles();
			floorMap.ClearAllTiles();
			navmesh.RemoveData();

			height = 45;
			width = 80;

			for(int i = 0; i < rooms.Count; i++){

				Room room = rooms[i];
				int attempt = 0;
				
				Vector3Int startCoord = new Vector3Int(Random.Range(0, width - room.roomWidth), Random.Range(0, height - room.roomHeight), 0);
				room.SetStartCoord(startCoord);

				while (!Validate(room))
				{
					startCoord = new Vector3Int(Random.Range(0, width - room.roomWidth), Random.Range(0, height - room.roomHeight), 0);
					room.SetStartCoord(startCoord);
					attempt++;
					if (attempt == 49)
					{
						return;
					}
				}

				rooms[i] = room;

				GenerateRoom(rooms[i].roomWidth, rooms[i].roomHeight, rooms[i].startCoord);
			}

			ConnectPassageways();
			FillRoom();
			navmesh.BuildNavMesh();
		}*/

		public void Shuffle(List<Room> ts)
		{
			int count = ts.Count-1;
			for (int i = 0; i < count; i++)
			{
				int r = UnityEngine.Random.Range(i, count);
				var tmp = ts[i];
				ts[i] = ts[r];
				ts[r] = tmp;
			}
		}
	}
}