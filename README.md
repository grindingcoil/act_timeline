act_timeline
============
レイドボスの攻撃パターン(タイムライン)を画面に表示するACTプラグインです。

## これはなに？
act_timelineは[ACT](http://advancedcombattracker.com/)用のプラグインです。

act_timelineを導入すると、ボスの攻撃パターンをオーバーレイとして画面上に表示させることができます:

![screenshot](https://raw.githubusercontent.com/grindingcoil/act_timeline/master/doc/scrshot.gif)

また、任意の攻撃の前に警告音を鳴らすことができます。

## インストール方法
1. ACTを導入する。
2. [act_timeline.zip](https://github.com/grindingcoil/act_timeline/blob/master/act_timeline.zip?raw=true) をダウンロードし、適当なフォルダに展開する。
3. ACTを起動する。

![Plugin Listing](https://raw.githubusercontent.com/grindingcoil/act_timeline/master/doc/install1.png)

4. Pluginタブ、Plugin Listingタブから[Browse..]ボタンをクリックする。
5. 展開したフォルダから bin\BindingCoil.ACTTimeline.dll を選択する。
6. [Add/Enable Plugin]ボタンをクリックする。
7. Pluginタブ、ACT Timelineタブを開く

![ACT Timeline Tab](https://raw.githubusercontent.com/grindingcoil/act_timeline/master/doc/install2.png)

8. Resources Directory: の [...] ボタンをおし、展開したフォルダの中にある resources フォルダを選択する。
9. Move by drag にチェックをいれた状態でタイムライン表示オーバーレイをドラッグして好きな場所に配置する。いい感じの場所に移動したらチェックをはずすと位置が固定される。
10. Number of rows to displayではオーバーレイの高さを設定できる。

## アンインストール方法
1. Pluginタブ、Plugin Listingタブで表示されているプラグイン一覧から BindingCoil.ACTTimeline.dllを探し、赤い×ボタンを押す。
2. 展開したフォルダを削除する。
3. %AppData%\Advanced Combat Tracker\Config フォルダにある ACTTimeline.config.xml にある設定ファイルを削除する。

## 使い方
![usage](https://raw.githubusercontent.com/grindingcoil/act_timeline/master/doc/usage.png)

## ライセンス
ソースコード/タイムラインtxtファイルは三条項BSDライセンスです。

タイムラインの追加・改善、バグの修正、機能追加をされた場合、プルリクエストを送っていただけると作者は大変喜びます。ただし強制はしません。

ただし付属wavファイルは魔王魂さんによるフリー素材の再配布です：
> サイト名：音楽素材/魔王魂

> トップページのURL:  http://maoudamashii.jokersounds.com/

> こちらの利用に際しては、[素材利用規約](http://maoudamashii.jokersounds.com/music_rule.html)に沿ってください。
