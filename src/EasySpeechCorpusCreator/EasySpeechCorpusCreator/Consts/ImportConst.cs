namespace EasySpeechCorpusCreator.Consts
{
    public static class ImportConst
    {
        // 設定値 説明
        public const string EXPLAIN_IMPORT_PASS = "● インポートする原稿ファイルのパス：";
        public const string EXPLAIN_IMPORT_PEEK = "● 原稿ファイル中身（3行まで）：";
        public const string EXPLAIN_IMPORT_FORMAT = "● インポート時の形式：\n初期入力値を参考に「変数を囲む区切り文字」と「原稿ファイルの形式」を入力してください。（初期値はITAコーパス）\n　例：%Name%,%Sentence%,%Kana%,%_Invalid%";
        public const string EXPLAIN_IMPORT_NAME = "● プロジェクト名：";

        // デフォルト値
        public const string DEFAULT_IMPORT_VARIABLE_CHAR = "%";
        public const string DEFAULT_IMPORT_FORMAT = "%Name%:%Sentence%,%Kana%";
        public const string DEFAULT_IMPORT_NAME = "〇〇コーパス";
    }
}
