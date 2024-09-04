using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Globalization;

namespace Localization
{

    [RequireComponent(typeof(Text))]
    public class TranslatedText : MonoBehaviour
    {
        [SerializeField]
        private string key;
        [SerializeField]
        private TextFormatting format;
        [SerializeField][Tooltip("Use the Text component font instead of the LanguageManager font")]
        private bool useTextFont = false;

        private Text text;
        private bool skipEnable = true;
        [SerializeField]
        private bool _isHeader;

        void Start()
        {
            text = GetComponent<Text>();
            UpdateText();
            skipEnable = false;
        }

        void OnEnable()
        {
            if (skipEnable)
                return;

            UpdateText();
        }

        public void UpdateText()
        {
            if(!useTextFont && this.text!=null)
                this.text.font = LanguageManager.GetFont();
            string text = LanguageManager.Get(key);
            if (this.text != null)
            {
                this.text.font = ThemesManager.Instance.CurrentTheme.ThemeFont;
                this.text.color = ThemesManager.Instance.CurrentTheme.ThemeFontColor;
            }
            
            TextInfo ti = CultureInfo.CurrentCulture.TextInfo;

            if (ThemesManager.Instance.CurrentTheme.ThemeType == ThemesManager.Themes.Doodle && this.text!=null && _isHeader)
            {
                this.text.color = Color.white;
            }
            if (this.text != null)
            {
                switch (format)
                {
                    case TextFormatting.Unchanged:
                        this.text.text = text;
                        break;
                    case TextFormatting.UpperCase:
                        this.text.text = text.ToUpper();
                        break;
                    case TextFormatting.LowerCase:
                        this.text.text = text.ToLower();
                        break;
                    case TextFormatting.TitleCase:
                        this.text.text = ti.ToTitleCase(text);
                        break;
                    case TextFormatting.SentenceCase:
                        if (text.Length > 0)
                            this.text.text = ti.ToUpper(text[0]).ToString();
                        if (text.Length >= 2)
                            this.text.text += text.Substring(1);
                        break;
                }
            }
            

        }

    }

}