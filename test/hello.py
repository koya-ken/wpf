import argparse
from fontTools.ttLib import TTFont


FONT_SPECIFIER_NAME_ID = 4
FONT_SPECIFIER_FAMILY_ID = 1


def get_font_name(font_path):
    """Get the short name from the font's names table"""
    name = ""
    family = ""
    # フォントファイルを読み込む
    font = TTFont(font_path)

    # フォント名を格納している 'name' テーブルを取得
    for record in font["name"].names:
        if b"\x00" in record.string:
            name_str = record.string.decode("utf-16-be")
        else:
            name_str = record.string.decode("utf-8")
        if record.nameID == FONT_SPECIFIER_NAME_ID and not name:
            name = name_str
        elif record.nameID == FONT_SPECIFIER_FAMILY_ID and not family:
            family = name_str
        if name and family:
            break
    return name, family

    # フォント名の取得（ID 4 は フォント名を格納しているエントリ）
    # for record in name_table.get:
    #     if record.nameID == 4:
    #         # フォント名をデコードして返す
    #         return record.toStr()

    return None


def main():
    # コマンドライン引数をパースする
    parser = argparse.ArgumentParser(description="Extract font name from a TTF file")
    parser.add_argument("font_path", help="Path to the TTF or OTF font file")
    args = parser.parse_args()

    # フォント名を取得して表示
    font_name = get_font_name(args.font_path)
    if font_name:
        print(f"Font Name: {font_name}")
    else:
        print("Font name not found.")


if __name__ == "__main__":
    main()
