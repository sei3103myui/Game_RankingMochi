<?php
//DB情報読み込み
require_once('config.php');

//GetParameterのScoreを取得
if(isset($_POST['mode'])){
    $mode = $_POST['mode'];
}else{
    http_response_code(400);
    die("Not Parameter Mode");
}

try{
    //
    $pdo = new PDO($dsn, $db_user, $db_password);

    //
    $sql = 'SELECT * FROM `2021_4193321_ranking` WHERE mode = :mode ORDER BY score DESC LIMIT 10;';
    $data = $pdo->prepare($sql);
    $data->bindValue(':mode',$mode,PDO::PARAM_STR);
    $data->execute();

    if(!empty($data)){
        foreach($data as $value){
            //
            $result[] = array(
                "id" => (int)$value["id"],
                "mode" => (int)$value["id"],
                "name" => $value["name"],
                "score" => (int)$value["score"]
            );
        }
    }
}catch(PDOException $e){
    //
    http_response_code(500);
    die($e->getMessage());
}

//
echo json_encode($result);

//
$pdo = null;