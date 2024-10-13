using UnityEngine;

namespace FPSMeleeDemo.FPS
{
	public class WeaponInstance : MonoBehaviour
	{
		public GameObject Graphics { get; private set; }

		public IWeapon Weapon { get; private set; }

		public static WeaponInstance Create(IWeapon weapon, Transform weaponPoint)
		{
			var inst = new GameObject("WeaponInstance").AddComponent<WeaponInstance>();

			inst.Graphics = Instantiate(weapon.Graphics, inst.transform);
			inst.Weapon = weapon;

			inst.transform.SetParent(weaponPoint);
			inst.transform.localPosition = Vector3.zero;
			inst.transform.localEulerAngles = Vector3.zero;

			return inst;
		}
	}
}

