# AR 개발환경 세팅

## 기기 호환 체크

- [AR Core : 지원되는 기기](https://developers.google.com/ar/devices?hl=ko)

## Android 개발자 모드 활성화

- 시스템 설정 -> 휴대전화 정보 -> 소프트웨어 정보 -> 빌드 번호 연타 -> 개발자 모드 활성화
- 시스템 설정 -> 개발자 옵션 -> USB 디버깅 활성화
- USB 연결 -> 연결(데이터 전송) 허용

## Unity Project

> ### 해상도 설정
>
> - 해두면 편함
> - Game View -> Free Aspect -> 커스텀 해상도 설정

### Package Import

- Unity Registry
  - AR Foundation
  - Google AR Core XR Plugin

### Build Settings

- Switch Android Platform

### Project Settings

#### XR Plug-in Management

- Android -> Google ARCore 체크
- Project Validation -> 경고 및 에러 Fix all

#### Player

- Graphics API : Vulkan 삭제
- Identification -> Minimum API Level 확인
- Configuration
  - Scriptng Backend -> IL2CPP
  - (GPS 사용할거라면) Active Input Handling -> Both
- Android Application Configuration
  - Targget Architectures -> ARM64 체크

#### Graphics

- Default Render Pipeline -> Mobile
  - 대응하는 Renderer -> `AR Background Render Feature`

### Scene

- 'Main Camera' 삭제
- Create -> XR -> AR Session 추가
  - 모바일 기기와 통신하면서 AR 서비스가 유지되도록 함
- Create -> XR -> XR Origin 추가
  - 유니티를 기준으로 동작하는 오브젝트