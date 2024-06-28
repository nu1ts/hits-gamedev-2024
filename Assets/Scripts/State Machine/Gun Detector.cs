using UnityEngine;

namespace State_Machine
{
    public class GunDetector : MonoBehaviour
    {
        public PlayerWeaponController weaponController;
        public AIWeaponController aiWeaponController;

        public bool isGunEquipped;
        private void FixedUpdate()
        {
            if (weaponController && !aiWeaponController)
            {
                isGunEquipped = weaponController.currentWeapon;
            }
            else if (!weaponController && aiWeaponController)
            {
                isGunEquipped = aiWeaponController.currentWeapon;
            }
        }
    }
}