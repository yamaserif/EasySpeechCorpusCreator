namespace EasySpeechCorpusCreator.Consts
{
    public static class SettingsConst
    {
        public const string SETTINGS_PATH = @".\..\settings.json";
        public const string SETTINGS_FORMAT = "UTF-8";

        // 設定値 説明
        public const string EXPLAIN_PROJECT_PASS = "プロジェクトのパス：\nコーパスのデータを管理するフォルダを設定します。\nコーパスを公開する際はこちらの中身をそのまま使用できます。";
        public const string EXPLAIN_FORMAT = "コーパスのフォーマット：\nコーパス管理データの形式を設定します。";

        // デフォルト値
        public const string DEFAULT_PROJECT_PASS = @".\..\projects\";
        public const Format DEFAULT_FORMAT = Format.CSV;


        // コーパス情報 保存フォーマット
        public enum Format
        {
            CSV,
            JSON
        }
    }
}
