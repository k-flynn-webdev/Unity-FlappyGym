using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UI_FPS : MonoBehaviour
{

	public Text _text_FPS;
	public Text _text_FPS_GC;


	private int _frameRate;
	private int _previousFrameRate = 59;
	private int _garbageCollections = 59;

	private float deltaTime;
	public float _updateTime = 0.1f;
	private float _internalTime = 0f;

	//	private float _mbUsed = 0f; // memory used by scripting engine, not total, so an overhead ontop of Unity its self..
	public Color _bestColour;
	public Color _worstColour;

	//private float _colorChange = 0f;

	static string[] stringsFrom00To99 = {
		"00", "01", "02", "03", "04", "05", "06", "07", "08", "09",
		"10", "11", "12", "13", "14", "15", "16", "17", "18", "19",
		"20", "21", "22", "23", "24", "25", "26", "27", "28", "29",
		"30", "31", "32", "33", "34", "35", "36", "37", "38", "39",
		"40", "41", "42", "43", "44", "45", "46", "47", "48", "49",
		"50", "51", "52", "53", "54", "55", "56", "57", "58", "59",
		"60", "61", "62", "63", "64", "65", "66", "67", "68", "69",
		"70", "71", "72", "73", "74", "75", "76", "77", "78", "79",
		"80", "81", "82", "83", "84", "85", "86", "87", "88", "89",
		"90", "91", "92", "93", "94", "95", "96", "97", "98", "99",
		"100", "101", "102", "103", "104", "105", "106", "107", "108", "109",
		"110", "111", "112", "113", "114", "115", "116", "117", "118", "119",
		"120", "121", "122", "123", "124", "125", "126", "127", "128", "129",
		"130", "131", "132", "133", "134", "135", "136", "137", "138", "139",
		"140", "141", "142", "143", "144", "145", "146", "147", "148", "149",
		"150", "151", "152", "153", "154", "155", "156", "157", "158", "159",
		"160", "161", "162", "163", "164", "165", "166", "167", "168", "169",
		"170", "171", "172", "173", "174", "175", "176", "177", "178", "179",
		"180", "181", "182", "183", "184", "185", "186", "187", "188", "189",
		"190", "191", "192", "193", "194", "195", "196", "197", "198", "199",
	};

	void OnEnable()
	{
		_internalTime = Time.time;
		_frameRate = 0;
		_previousFrameRate = 0;
		_garbageCollections = 0;
		deltaTime = 0f;
		_text_FPS_GC.text = stringsFrom00To99[_garbageCollections];
		_text_FPS.text = stringsFrom00To99[_frameRate]; // efficent way
	}

	void LateUpdate()
	{
		deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
		_frameRate = (int)(1.0f / deltaTime);
		_frameRate = Mathf.Clamp(_frameRate, 1, 199);


		if (Time.time >= (_internalTime + _updateTime))
		{
			_text_FPS.text = stringsFrom00To99[_frameRate]; // efficent way

			if (_previousFrameRate - _frameRate > 3)
			{
				_garbageCollections += 1;
				if (_garbageCollections == 99)
				{
					_garbageCollections = 0;
				}


				_text_FPS_GC.text = stringsFrom00To99[_garbageCollections];
			}

			_internalTime = Time.time;
			_previousFrameRate = _frameRate;

			//if (_internalTime >= _colorChange)
			//{
				float colorVal = _frameRate / 100f;
				_text_FPS.color = Color.Lerp(_worstColour, _bestColour, colorVal);
				//_colorChange = _internalTime + 2f;
			//}
		}


	}
}