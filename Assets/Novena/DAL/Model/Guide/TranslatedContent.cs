#nullable enable
using System.Collections.Generic;
using System.Linq;

namespace Novena.DAL.Model.Guide {
	[System.Serializable]
	public class TranslatedContent {
		public int Id;
		public int LanguageId;
		public int Rank;
		public string ContentTitle;
		public string LanguageName;
		public string LanguageEnglishName;
		public string LanguageThumbnailPath;
		public string LanguageThumbnailTimestamp;
		public Theme[] Themes;
		public TagCategorie[] TagCategories;

		#region Helper methods

		public Theme? GetThemeByLanguageSwitchCode(int code)
		{
			Theme? output = null;

			output = Themes.FirstOrDefault(t => t.LanguageSwitchCode == code);

			return output;
		}

		public Theme? GetThemeByTag(string tagName)
		{
			Theme? output = null;

			output = Themes.FirstOrDefault(t => t.Tags != null && t.Tags.Any(tag => tag.Title == tagName));

			return output;
		}

		public List<Theme> GetThemesByTag(string tagName)
		{
			List<Theme> themes = null;

			themes = Themes.Where(t => t.Tags != null && t.Tags.Any(tag => tag.Title == tagName)).ToList();

			return themes;
		}

		/// <summary>
		/// Get theme by name.
		/// </summary>
		/// <param name="name">Theme name</param>
		/// <returns>Theme if found or null</returns>
		public Theme? GetThemeByName(string name)
		{
			Theme? output = null;

			output = Themes?.FirstOrDefault(theme => theme.Name == name);

			return output;
		}

		/// <summary>
		/// Get theme by label.
		/// </summary>
		/// <param name="label">Label name</param>
		/// <returns>Theme if found or null</returns>
		public Theme? GetThemeByLabel(string label)
		{
			Theme? output = null;

			output = Themes?.FirstOrDefault(theme => theme.Label == label);

			return output;
		}

		/// <summary>
		/// Get list of theme that have no tag or excluded tag name.
		/// </summary>
		/// <param name="excludeTagName"></param>
		/// <returns></returns>
		public List<Theme> GetThemesExcludeByTag(string excludeTagName)
		{
			List<Theme> output = new List<Theme>();

			output = Themes?.Where(t => t.Tags == null || t.Tags.All(tag => tag.Title != excludeTagName)).ToList();

			return output;
		}

		/// <summary>
		/// Get tag category by name.
		/// </summary>
		/// <param name="categoryName"></param>
		/// <returns>TagCategorie or NULL if nothing found!</returns>
		public TagCategorie? GetTagCategoryByName(string categoryName)
		{
			return TagCategories?.FirstOrDefault(cat => cat.Title == categoryName);
		}

		#endregion
	}
}