using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.IO;
using System.Net.Sockets;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Vehicles.Car;
using UnityEditor.SceneManagement;
using UnityEditor;
 

public class networkSocket : MonoBehaviour
{
    public String host = "localhost";
    public Int32 port = 50000;

	int a;
	int stuck_counter;
	float last_velocity;
	//bool correct_direction;
	//Vector3[] Waypoints;
	//int i;
	//bool restarted;

	public bool done;
	public bool quit;

    internal Boolean socket_ready = false;
    internal String input_buffer = "";
    TcpClient tcp_socket;
    NetworkStream net_stream;

    StreamWriter socket_writer;
    StreamReader socket_reader;

	Text [] actions_UI = new Text[9];
	bool finished = false;

    void UpdateMe()
    {		
		String message = readSocket ();
		//Debug.Log (readSocket());
		if (message == "Send the starting State") {
			// Debug.Log ("arrived");
			if (finished) {
				finished = false;
				writeSocket ("finished");
			}
			if (quit) {
				quit = false;
				writeSocket ("quit");
			}
			if (State_is_done () || done) {
				done = false;
				writeSocket ("done");
			} else {
				// Debug.Log ("else part");
				writeSocket (getCurrentState ());
			}
		} else if(message == "restart"){
			writeSocket ("oksh");
			//restarted = true;
			EditorSceneManager.LoadScene ("demo");
			//Waypoints = GameObject.Find ("StreetManger (1)").GetComponent<CityDesgin1> ().Waypoints;
		} else if(message == "Bye!"){
//			Application.Quit();
			UnityEditor.EditorApplication.isPlaying = false;
		}else{
			Debug.Log (message);
			a = Int32.Parse(message.Substring (10,1));
			// Debug.Log (a);
			DoAction (a);
			writeSocket ("action done");
		}

		if ((float)(Math.Round ((double)GameObject.Find ("Car(Clone)").transform.GetComponent<Rigidbody> ().velocity.z, 2)) == (float)(Math.Round ((double)last_velocity, 2))) {
			if ((float)(Math.Round ((double)last_velocity, 1)) == 0f) {
				stuck_counter++;
			}
		} else {
			stuck_counter = 0;
		}

		
    }

	void Update(){
		DoAction (a);
		/*if(restarted){
			i = 1;
		}
		Vector3 CarToWaypoint = GameObject.Find ("Car(Clone)").transform.position - Waypoints [i];

		if(CarToWaypoint.magnitude < 7 ){
			i += 1;
		}

		Vector3 direction = Waypoints [i] - Waypoints [i - 1];
		print ("i = " + i);
		if (Vector3.Angle (GameObject.Find ("Car(Clone)").transform.forward, direction) <= 80) {
			correct_direction = true;
		} else {
			correct_direction = false;
		}*/
	}

	/*void putWaypoints(){
		if (restarted) {
			Waypoints = GameObject.FindGameObjectWithTag ("manager").GetComponent<CityDesgin1> ().Waypoints;
			restarted = false;
		}
	}*/

    void Awake()
    {
		//Waypoints = GameObject.FindGameObjectWithTag("manager").GetComponent<CityDesgin1> ().Waypoints;
		actions_UI[0] = GameObject.Find ("Canvas/None").GetComponent<Text> ();
		actions_UI[1] = GameObject.Find ("Canvas/Forward").GetComponent<Text> ();
		actions_UI[2] = GameObject.Find ("Canvas/Backward").GetComponent<Text> ();
		actions_UI[3] = GameObject.Find ("Canvas/Right").GetComponent<Text> ();
		actions_UI[4] = GameObject.Find ("Canvas/Left").GetComponent<Text> ();
		actions_UI[5] = GameObject.Find ("Canvas/Forward_Right").GetComponent<Text> ();
		actions_UI[6] = GameObject.Find ("Canvas/Backward_Right").GetComponent<Text> ();
		actions_UI[7] = GameObject.Find ("Canvas/Forward_Left").GetComponent<Text> ();
		actions_UI[8] = GameObject.Find ("Canvas/Backward_Left").GetComponent<Text> ();

		GameObject[] objs = GameObject.FindGameObjectsWithTag ("Network");
		if (objs.Length > 1) {
			Destroy (this.gameObject);
		}

		DontDestroyOnLoad (this.gameObject);
        setupSocket();
		//i = 1;
		a = 0;
		stuck_counter = 0;
		//restarted = true;
        InvokeRepeating("UpdateMe", 3f, 0.2f);
		InvokeRepeating("putWaypoints", 3f, 0.2f);
    }

    void OnApplicationQuit()
    {
        closeSocket();
    }

    public void setupSocket()
    {
        try
        {
            tcp_socket = new TcpClient(host, port);

            net_stream = tcp_socket.GetStream();
            socket_writer = new StreamWriter(net_stream);
            socket_reader = new StreamReader(net_stream);

            socket_ready = true;
        }
        catch (Exception e)
        {
        	// Something went wrong
            Debug.Log("Socket error: " + e);
        }
    }

    public void writeSocket(string line)
    {
        if (!socket_ready)
            return;
            
        line = line + "\r\n";
        socket_writer.Write(line);
        socket_writer.Flush();
    }

    public String readSocket()
    {
        if (!socket_ready)
        {
            Debug.Log("socket not ready");
            return "";
        }

        if (net_stream.DataAvailable)
            return socket_reader.ReadLine();

        return "";
    }

    public void closeSocket()
    {
        if (!socket_ready)
            return;

        socket_writer.Close();
        socket_reader.Close();
        tcp_socket.Close();
        socket_ready = false;
	}

	public void set_finished(){
		finished = true;
	}


	public String getCurrentState(){
		float[] state_output = new float[46];
		/*for (int i = 0; i <= 35; i++) {
			Debug.Log (i);
			state_output [i] = GameObject.Find("StreetManger (1)").GetComponent<CityDesgin1> ().Car.GetComponent<RecordingScript> ().lines [i].distance;
		}*/

		//Sensor obstacle distance
		int i = 0;
		state_output[i++] = SensorsGlobalManager.Instance.distance0;
		state_output[i++] = SensorsGlobalManager.Instance.distance10;
		state_output[i++] = SensorsGlobalManager.Instance.distance20;
		state_output[i++] = SensorsGlobalManager.Instance.distance30;
		state_output[i++] = SensorsGlobalManager.Instance.distance40;
		state_output[i++] = SensorsGlobalManager.Instance.distance50;
		state_output[i++] = SensorsGlobalManager.Instance.distance60;
		state_output[i++] = SensorsGlobalManager.Instance.distance70;
		state_output[i++] = SensorsGlobalManager.Instance.distance80; 
		state_output[i++] = SensorsGlobalManager.Instance.distance90;
		state_output[i++] = SensorsGlobalManager.Instance.distance100;
		state_output[i++] = SensorsGlobalManager.Instance.distance110;
		state_output[i++] = SensorsGlobalManager.Instance.distance120;
		state_output[i++] = SensorsGlobalManager.Instance.distance130;
		state_output[i++] = SensorsGlobalManager.Instance.distance140;
		state_output[i++] = SensorsGlobalManager.Instance.distance150;
		state_output[i++] = SensorsGlobalManager.Instance.distance160;
		state_output[i++] = SensorsGlobalManager.Instance.distance170;
		state_output[i++] = SensorsGlobalManager.Instance.distance180;
		state_output[i++] = SensorsGlobalManager.Instance.distance190;
		state_output[i++] = SensorsGlobalManager.Instance.distance200;
		state_output[i++] = SensorsGlobalManager.Instance.distance210;
		state_output[i++] = SensorsGlobalManager.Instance.distance220;
		state_output[i++] = SensorsGlobalManager.Instance.distance230;
		state_output[i++] = SensorsGlobalManager.Instance.distance240;
		state_output[i++] = SensorsGlobalManager.Instance.distance250;
		state_output[i++] = SensorsGlobalManager.Instance.distance260;
		state_output[i++] = SensorsGlobalManager.Instance.distance270;
		state_output[i++] = SensorsGlobalManager.Instance.distance280;
		state_output[i++] = SensorsGlobalManager.Instance.distance290;
		state_output[i++] = SensorsGlobalManager.Instance.distance300;
		state_output[i++] = SensorsGlobalManager.Instance.distance310;
		state_output[i++] = SensorsGlobalManager.Instance.distance320;
		state_output[i++] = SensorsGlobalManager.Instance.distance330;
		state_output[i++] = SensorsGlobalManager.Instance.distance340;
		state_output[i++] = SensorsGlobalManager.Instance.distance350;

		//Car velocity
		//state_output [36] = (float) GameObject.Find("Car(Clone)").transform.InverseTransformDirection(GameObject.Find("Car(Clone)").transform.GetComponent<Rigidbody>().velocity).x;
		//state_output [37] = (float) GameObject.Find("Car(Clone)").transform.InverseTransformDirection(GameObject.Find("Car(Clone)").transform.GetComponent<Rigidbody>().velocity).y;
		state_output [36] = (float) GameObject.Find("Car(Clone)").transform.InverseTransformDirection(GameObject.Find("Car(Clone)").transform.GetComponent<Rigidbody>().velocity).z;
		// Debug.Log(state_output [38]);
		last_velocity = state_output [36];
		//state_output [39] = GameObject.Find("StreetManger (1)").GetComponent<CityDesgin1> ().Car.transform.eulerAngles.y - GameObject.Find("StreetManger (1)").GetComponent<CityDesgin1>().Car.GetComponent<RecordingScript>().getRoadBlock().transform.eulerAngles.y;

		//Car Angle
		state_output [37] = GameObject.Find("Car(Clone)").GetComponent<RecordingScript>().CarAngle * (180/Mathf.PI) / 10;

		if (GameObject.Find("Car(Clone)").GetComponent<RecordingScript> ().trafficLights ()) {
			state_output [38] = 1f;
		} else {
			state_output [38] = 0f;
		}

		if (GameObject.Find("StreetManger (1)").GetComponent<CityDesgin1>().isRainy) {
			state_output [39] = 1f;
		} else {
			state_output [39] = 0f;
		}

		state_output [40] = (float) GameObject.Find("Car(Clone)").GetComponent<RecordingScript> ().collidedObstacles.Count;
		state_output [41] = (float) GameObject.Find("Car(Clone)").GetComponent<RecordingScript> ().collidedPedestrians.Count;

		//Pavement-hit
		if (GameObject.Find("Car(Clone)").GetComponent<RecordingScript> ().collidedPavement) {
			state_output [42] = 1f;
		} else {
			state_output [42] = 0f;
		}

		if (GameObject.Find("CarDirection").GetComponent<testScript>().DirectionAngle <= 80) {
			state_output [43] = 1f;
		} else {
			state_output [43] = 0f;
		}

		//adversarial distance
		state_output[44] = 0f;

		//adversarial ahead or not
		state_output[45] = 0f;

		String output = "";
		for (int j = 0 ; j < state_output.Length ; j++){
			output = output + Convert.ToString (state_output [j]) + ",";
		}

		return output;
	}

	public bool State_is_done() //COMPLETE THE CODE
	{
		if (GameObject.Find ("Car(Clone)").transform.position.y < -3) {
			return true;
		} else if (stuck_counter >= 10 && GameObject.Find("Car(Clone)").GetComponent<RecordingScript> ().trafficLights () == false) {
			stuck_counter = 0;
			return true;
		} else {
			return false;
		}
	}

	public void DoAction(int action_number) 
	{
		// Debug.Log ("hi action");
		for (int i = 0; i < actions_UI.Length; i++) {
			actions_UI[i].fontSize = 16;
			actions_UI[i].fontStyle = FontStyle.Normal;
			actions_UI[i].color = Color.white;
		}

		actions_UI[action_number].fontSize = 18; 
		actions_UI[action_number].fontStyle = FontStyle.Bold;
		actions_UI[action_number].color = Color.red;

		if (action_number == 0) {
			GameObject.Find("Car(Clone)").GetComponent<CarRemoteControl> ().SteeringAngle = 0f;
			GameObject.Find("Car(Clone)").GetComponent<CarRemoteControl> ().Acceleration = 0f;
			//CrossPlatformInputManager.SetAxis ("Horizontal", 0f);
			//CrossPlatformInputManager.SetAxis ("Vertical", 0f);
		}

		if (action_number == 1) {
			GameObject.Find("Car(Clone)").GetComponent<CarRemoteControl> ().SteeringAngle = 0f;
			GameObject.Find("Car(Clone)").GetComponent<CarRemoteControl> ().Acceleration = 1f;
			//CrossPlatformInputManager.SetAxis ("Horizontal", 0f);
			//CrossPlatformInputManager.SetAxis ("Vertical", 1f);
		}

		if (action_number == 2) {
			GameObject.Find("Car(Clone)").GetComponent<CarRemoteControl> ().SteeringAngle = 0f;
			GameObject.Find("Car(Clone)").GetComponent<CarRemoteControl> ().Acceleration = -1f;
			//CrossPlatformInputManager.SetAxis ("Horizontal", 0f);
			//CrossPlatformInputManager.SetAxis ("Vertical", -1f);
		}

		if (action_number == 3) {
			GameObject.Find("Car(Clone)").GetComponent<CarRemoteControl> ().SteeringAngle = 1f;
			GameObject.Find("Car(Clone)").GetComponent<CarRemoteControl> ().Acceleration = 0f;
			//CrossPlatformInputManager.SetAxis ("Horizontal", 1f);
			//CrossPlatformInputManager.SetAxis ("Vertical", 0f);
		}

		if (action_number == 4) {
			GameObject.Find("Car(Clone)").GetComponent<CarRemoteControl> ().SteeringAngle = -1f;
			GameObject.Find("Car(Clone)").GetComponent<CarRemoteControl> ().Acceleration = 0f;
			//CrossPlatformInputManager.SetAxis ("Horizontal", -1f);
			//CrossPlatformInputManager.SetAxis ("Vertical", 0f);
		}

		if (action_number == 5) {
			GameObject.Find("Car(Clone)").GetComponent<CarRemoteControl> ().SteeringAngle = 1f;
			GameObject.Find("Car(Clone)").GetComponent<CarRemoteControl> ().Acceleration = 1f;
			//CrossPlatformInputManager.SetAxis ("Horizontal", 1f);
			//CrossPlatformInputManager.SetAxis ("Vertical", 1f);
		}

		if (action_number == 6) {
			GameObject.Find("Car(Clone)").GetComponent<CarRemoteControl> ().SteeringAngle = 1f;
			GameObject.Find("Car(Clone)").GetComponent<CarRemoteControl> ().Acceleration = -1f;
			//CrossPlatformInputManager.SetAxis ("Horizontal", 1f);
			//CrossPlatformInputManager.SetAxis ("Vertical", -1f);
		}

		if (action_number == 7) {
			GameObject.Find("Car(Clone)").GetComponent<CarRemoteControl> ().SteeringAngle = -1f;
			GameObject.Find("Car(Clone)").GetComponent<CarRemoteControl> ().Acceleration = 1f;
			//CrossPlatformInputManager.SetAxis ("Horizontal", -1f);
			//CrossPlatformInputManager.SetAxis ("Vertical", 1f);
		}

		if (action_number == 8) {
			GameObject.Find("Car(Clone)").GetComponent<CarRemoteControl> ().SteeringAngle = -1f;
			GameObject.Find("Car(Clone)").GetComponent<CarRemoteControl> ().Acceleration = -1f;
			//CrossPlatformInputManager.SetAxis ("Horizontal", -1f);
			//CrossPlatformInputManager.SetAxis ("Vertical", -1f);
		}
	}

}