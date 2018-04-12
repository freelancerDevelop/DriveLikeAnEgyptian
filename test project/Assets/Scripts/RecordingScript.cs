﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace UnityStandardAssets.Vehicles.Car
{
	[RequireComponent(typeof(CarController))]
	public class RecordingScript : MonoBehaviour {

		public Line [] lines; 

		string logFilePath;

		void Awake (){
			print (Application.dataPath);
			logFilePath = Application.dataPath + "/LogFiles/state-action.json";
		}

		// Use this for initialization
		void Start () {
			lines = new Line[36];
			print (Application.persistentDataPath);
		}
		
		// Update is called once per frame
		void Update () {
			//		JsonUtility.ToJson(obj[i]) + "*";

			State currentState = new State()
			{
				line0 = new Line()
				{
					type = SensorsGlobalManager.Instance.type0,
					position = SensorsGlobalManager.Instance.position0
				},
				line5 = new Line()
				{
					type = SensorsGlobalManager.Instance.type5,
					position = SensorsGlobalManager.Instance.position5
				},
				line10 = new Line()
				{
					type = SensorsGlobalManager.Instance.type10,
					position = SensorsGlobalManager.Instance.position10
				},
				line15 = new Line()
				{
					type = SensorsGlobalManager.Instance.type15,
					position = SensorsGlobalManager.Instance.position15
				},
				line20 = new Line()
				{
					type = SensorsGlobalManager.Instance.type20,
					position = SensorsGlobalManager.Instance.position20
				},
				line25 = new Line()
				{
					type = SensorsGlobalManager.Instance.type25,
					position = SensorsGlobalManager.Instance.position25
				},
				line30 = new Line()
				{
					type = SensorsGlobalManager.Instance.type30,
					position = SensorsGlobalManager.Instance.position30
				},
				line35 = new Line()
				{
					type = SensorsGlobalManager.Instance.type35,
					position = SensorsGlobalManager.Instance.position35
				},
				line40 = new Line()
				{
					type = SensorsGlobalManager.Instance.type40,
					position = SensorsGlobalManager.Instance.position40
				},
				line45 = new Line()
				{
					type = SensorsGlobalManager.Instance.type45,
					position = SensorsGlobalManager.Instance.position45
				},
				line50 = new Line()
				{
					type = SensorsGlobalManager.Instance.type50,
					position = SensorsGlobalManager.Instance.position50
				},
				line55 = new Line()
				{
					type = SensorsGlobalManager.Instance.type55,
					position = SensorsGlobalManager.Instance.position55
				},
				line60 = new Line()
				{
					type = SensorsGlobalManager.Instance.type60,
					position = SensorsGlobalManager.Instance.position60
				},
				line65 = new Line()
				{
					type = SensorsGlobalManager.Instance.type65,
					position = SensorsGlobalManager.Instance.position65
				},
				line70 = new Line()
				{
					type = SensorsGlobalManager.Instance.type70,
					position = SensorsGlobalManager.Instance.position70
				},
				line75 = new Line()
				{
					type = SensorsGlobalManager.Instance.type75,
					position = SensorsGlobalManager.Instance.position75
				},
				line80 = new Line()
				{
					type = SensorsGlobalManager.Instance.type80,
					position = SensorsGlobalManager.Instance.position80
				},
				line85 = new Line()
				{
					type = SensorsGlobalManager.Instance.type85,
					position = SensorsGlobalManager.Instance.position85
				},
				line90 = new Line()
				{
					type = SensorsGlobalManager.Instance.type90,
					position = SensorsGlobalManager.Instance.position90
				},
				line95 = new Line()
				{
					type = SensorsGlobalManager.Instance.type95,
					position = SensorsGlobalManager.Instance.position95
				},
				line100 = new Line()
				{
					type = SensorsGlobalManager.Instance.type100,
					position = SensorsGlobalManager.Instance.position100
				},
				line105 = new Line()
				{
					type = SensorsGlobalManager.Instance.type105,
					position = SensorsGlobalManager.Instance.position105
				},
				line110 = new Line()
				{
					type = SensorsGlobalManager.Instance.type110,
					position = SensorsGlobalManager.Instance.position110
				},
				line115 = new Line()
				{
					type = SensorsGlobalManager.Instance.type115,
					position = SensorsGlobalManager.Instance.position115
				},
				line120 = new Line()
				{
					type = SensorsGlobalManager.Instance.type120,
					position = SensorsGlobalManager.Instance.position120
				},
				line125 = new Line()
				{
					type = SensorsGlobalManager.Instance.type125,
					position = SensorsGlobalManager.Instance.position125
				},
				line130 = new Line()
				{
					type = SensorsGlobalManager.Instance.type130,
					position = SensorsGlobalManager.Instance.position130
				},
				line135 = new Line()
				{
					type = SensorsGlobalManager.Instance.type135,
					position = SensorsGlobalManager.Instance.position135
				},
				line140 = new Line()
				{
					type = SensorsGlobalManager.Instance.type140,
					position = SensorsGlobalManager.Instance.position140
				},
				line145 = new Line()
				{
					type = SensorsGlobalManager.Instance.type145,
					position = SensorsGlobalManager.Instance.position145
				},
				line150 = new Line()
				{
					type = SensorsGlobalManager.Instance.type150,
					position = SensorsGlobalManager.Instance.position150
				},
				line155 = new Line()
				{
					type = SensorsGlobalManager.Instance.type155,
					position = SensorsGlobalManager.Instance.position155
				},
				line160 = new Line()
				{
					type = SensorsGlobalManager.Instance.type160,
					position = SensorsGlobalManager.Instance.position160
				},
				line165 = new Line()
				{
					type = SensorsGlobalManager.Instance.type165,
					position = SensorsGlobalManager.Instance.position165
				},
				line170 = new Line()
				{
					type = SensorsGlobalManager.Instance.type170,
					position = SensorsGlobalManager.Instance.position170
				},
				line175 = new Line()
				{
					type = SensorsGlobalManager.Instance.type175,
					position = SensorsGlobalManager.Instance.position175
				},

				car_velocity = (GetComponent("CarController") as CarController).CurrentSpeed,
//				traffic_light =  GameObject.Find ("") //get current roadblock
			};

//			print (currentState_JSON+ "\t\n"	);
			Action newAction = new Action (){
				acceleration = (GetComponent("CarController") as CarController).AccelInput,
				steer_angle = (GetComponent("CarController") as CarController).CurrentSteerAngle
			};

			StateActionPair newPair = new StateActionPair() {
				a = newAction,
				s = currentState
			};

			string newPair_JSON = JsonUtility.ToJson(newPair,true);
			logStateAction (newPair_JSON, logFilePath);	

		}


		void logStateAction(string objects,string path){
			StreamWriter writer = new StreamWriter (path, true);
			writer.WriteLine (objects);
			writer.Close();
		}


		[System.Serializable]
		public class Line
		{
			public string type;
			public Vector3 position;
		}

		[System.Serializable]
		public class State
		{
			public Line line0;
			public Line line5;
			public Line line10;
			public Line line15;
			public Line line20;
			public Line line25;
			public Line line30;
			public Line line35;
			public Line line40;
			public Line line45;
			public Line line50;
			public Line line55;
			public Line line60;
			public Line line65;
			public Line line70;
			public Line line75;
			public Line line80;
			public Line line85;
			public Line line90;
			public Line line95;
			public Line line100;
			public Line line105;
			public Line line110;
			public Line line115;
			public Line line120;
			public Line line125;
			public Line line130;
			public Line line135;
			public Line line140;
			public Line line145;
			public Line line150;
			public Line line155;
			public Line line160;
			public Line line165;
			public Line line170;
			public Line line175;

			public bool rain;
			public float car_velocity;
			public float car_angle;
			public bool traffic_light;
		}

		[System.Serializable]
		public class Action
		{
			public float acceleration;
			public float steer_angle;

		}

		[System.Serializable]
		public class StateActionPair
		{
			public State s;
			public Action a;

		}
	}
}
