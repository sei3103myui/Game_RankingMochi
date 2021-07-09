<?php
//DB情報読み込み
require_once('config.php');

try{
    //
    $pdo = new PDO($dsn, $db_user, $db_password);

    //
    $sql = 'SELECT * FROM `2021_4193321_ranking` ORDER BY score DESC LIMIT 5;';
    $data = $pdo->query($sql);

    if(!empty($data)){
        foreach($data as $value){
            //
            $result[] = array(
                "id" => (int)$value["id"],
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