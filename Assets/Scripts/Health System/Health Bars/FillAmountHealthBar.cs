#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using UnityEngine.UI;


namespace Game.HealthSystem
{
    /// <summary>
    /// This Health Bar is Used for having slider like Health Bar
    /// </summary>
    public class FillAmountHealthBar : HealthBar<IntHealthSystem>
    {
        #region Variables
        [SerializeField] private Gradient _tintGradient;

        private Image _fillImage;
        private Image _maskImage;
        #endregion


        #region Getters And Setters

        #endregion


        #region Unity Calls
        #endregion


        #region Component Functions
#if UNITY_EDITOR
        [MenuItem("GameObject/Fill Amount Health Bar")]
        public static void CreateBar()
        {
            GameObject bar = Instantiate(Resources.Load<GameObject>("Health System/Fill Amount Health Bar"));
            bar.name = "HealthBar";
            bar.transform.SetParent(Selection.activeTransform);
        }
#endif

        public override void Setup(IntHealthSystem healthSystem)
        {
            _maskImage = transform.Find("Mask").GetComponent<Image>();
            _fillImage = _maskImage.transform.Find("Fill").GetComponent<Image>();
            _healthSystem = healthSystem;
            _healthSystem.OnDamaged += HealthChanged;
            _healthSystem.OnHealed += HealthChanged;
            HealthChanged(_healthSystem.Current);
        }
        private void HealthChanged(float changeAmount)
        {
            _maskImage.fillAmount = _healthSystem.Health01;
            _fillImage.color = _tintGradient.Evaluate(_healthSystem.Health01);
        }
        #endregion
    }
}