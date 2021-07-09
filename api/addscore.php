<?php
//DB情報読み込み（他のPHPファイル読み込み）
require_once('config.php');

//GetParameterのnameを取得
if(isset($_POST['name'])){
    $name = $_POST['name'];
}else{
    http_response_code(400);
    die("Not Parameter Name");
}

//GetParameterのScoreを取得
if(isset($_POST['score'])){
    $score = $_POST['score'];
}else{
    http_response_code(400);
    die("Not Parameter Score");
}

//GetParameterのScoreを取得
if(isset($_POST['mode'])){
    $mode = $_POST['mode'];
}else{
    http_response_code(400);
    die("Not Parameter Mode");
}


try{
    //DBへ接続
    $pdo = new PDO($dsn, $db_user, $db_password);

    //データの追加
    $stmt = $pdo->prepare("INSERT INTO `2021_4193321_ranking`(name,score,mode) VALUES (:name, :score, :mode)");
    $stmt->bindValue(':name', $name, PDO::PARAM_STR);
    $stmt->bindValue(':score', $score, PDO::PARAM_STR);
    $stmt->bindValue(':mode', $mode, PDO::PARAM_STR);
    $stmt->execute();
}catch(PDOException $e){
    //DBエラー
    http_response_code(500);
    die($e->getMessage());
}

echo "success";

//接続を閉じる
$pdo = null;
?>
