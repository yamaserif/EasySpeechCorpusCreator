namespace EasySpeechCorpusCreator.Consts
{
    public static class SettingsConst
    {
        public const string SETTINGS_PATH = @".\..\settings.json";
        public const string SETTINGS_FORMAT = "UTF-8";

        // 設定値 説明
        public const string EXPLAIN_PROJECT_PASS = "● プロジェクトのパス：\nコーパスのデータを管理するフォルダを設定します。\nコーパスを公開する際はこちらの中身をそのまま使用できます。";
        public const string EXPLAIN_FORMAT = "● コーパスのフォーマット：\nコーパス管理データの形式を設定します。(新しいプロジェクトから適用)";
        public const string EXPLAIN_EXTERNAL_SOFTWARE = "● 外部ソフト連携：\n音声ファイルの外部ソフト連携用コマンドを設定します。\n%VOICE_FILE%：コーパス音声データファイルのパス\n%S%：文字列としてのスペース";
        public const string EXPLAIN_DEVICE = "● 録音デバイス：\n録音に使用するデバイスを選択します。";
        public const string EXPLAIN_SAMPLING_RATE = "● サンプリングレート(Hz)：\n録音時のサンプリングレートを設定します";

        // デフォルト値
        public const string DEFAULT_PROJECT_PASS = @".\..\projects\";
        public const Format DEFAULT_FORMAT = Format.JSON;
        public const string DEFAULT_EXTERNAL_SOFTWARE = "explorer.exe \"%VOICE_FILE%\"";
        public const string DEFAULT_DEVICE = "";
        public const int DEFAULT_SAMPLING_RATE = 44100;

        public const string EXTERNAL_SOFTWARE_PATH = "%VOICE_FILE%";
        public const string EXTERNAL_SOFTWARE_SPACE = "%S%";

        // コーパス情報 保存フォーマット
        public enum Format
        {
            JSON,
            CSV
        }
    }
}
