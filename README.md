# tally-assets
ハンディー読込データ（csvファイル）から、部門別在庫金額を集計したエクセルファイルを生成します。

ハンディーデータには下記の3種類
- 自店で商品バーコードを読取った棚卸データ（TANALOG.DAT）
- 自店で部門、上代、数量を入力した棚卸データ（TANALOG2.DAT）
- 棚卸を外部委託した納品データ

# フォーマット
## TANALOG.DAT
| カラム位置 | 項目   | 入力例   |
|---:        |:---:   |:---:     |
| 1          | 年月日 | yyyymmdd |
| 2          | -      | |
| 3          | -      | |
| 4          | -      | |
| 5          | JAN    | |
| 6          | 数量   | |

## TANALOG2.DAT
|  カラム位置  |  項目     | 入力例   |
|---:          |:---:      |:---:     |
| 1            | 年月日    | yyyymmdd |
| 2            | -         | |
| 3            | -         | |
| 4            | -         | |
| 5            | 税込上代  | |
| 6            | 部門      | 201 |
| 7            | 掛率      | 55 |
| 8            | 数量      |  |

## 外部委託
|  カラム位置  |  項目     | 入力例 |
|---:          |:---:      |:---:   |
| 1            | JAN       |  |
| 2            | 掛率      | 55 |
| 3            | 部門      | 201 |
| 4            | 税込上代  |  |
| 5            | 数量      |  |

# 手順
1. 棚卸データを保存する場所の作成。
1. 棚卸データを所定の場所に保存します。
1. 集計表生成実行。

## 店舗別にフォルダツリーを生成する
C:\tmp 配下にフォルダツリーを作る場合
* init.cmd
* enumerableMkdir.js
1. 上記2ファイルを C:\tmp にコピーします。
1. init.cmd を実行します。
店舗数が増えた場合は、init.cmdの下記数字を書き換えます。

```bat:init.cmd
SET /A REPEAT=34
```

## 棚卸データを所定の場所に保存します
* HT\tanalog\配下の店舗別に、TANALOG.DAT、TANALOG2.DATを保存します。
* HT\outsourcing\配下の店舗別に、業者の棚卸データ（shift-jis、拡張子は無視）を店舗別に保存します。

```dos
HT
├─outsourcing
│  │─001
│  │      納品データ.CSV
└─tanalog
    │─001
    │      TANALOG.DAT
    │      TANALOG2.DAT
```

## 集計表生成
* run.cmd をメモ帳で開き、棚卸年・月、棚卸データ格納場所、集計結果格納場所を設定する。

```bat:run.cmd
REM 棚卸年・月
SET /A YEAR=2017
SET /A MONTH=8

REM 棚卸データの格納フォルダ
SET JUNCTION=C:\棚卸\HT\
SET TANALOG=%JUNCTION%tanalog\
SET OUTSOURCING=%JUNCTION%outsourcing\

REM 集計結果の格納フォルダ
SET OUTPUT=C:\棚卸\
```

* run.cmdを実行する
