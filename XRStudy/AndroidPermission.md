# 안드로이드 권한 요청

## 권한 요청을 하는 이유는?

- 카메라, 위치, 마이크, 저장 공간 등 민감함 기능에 접근해야 할 때, 사용자가 명시적으로 기능을 사용할 수 있게 허가하도록 하기 위함

## 매니페스트 (Manifest)

- 앱에서 사용할 권한을 미리 명시해둠

## 유니티에서 매니페스트 설정

- Project Settings -> Player -> Publish Settings -> Build -> Custom Main Manifest 체크
- Assets/Plugins/Android/Manifest.xml 경로로 생성