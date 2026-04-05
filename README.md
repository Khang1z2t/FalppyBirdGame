# FalppyBirdGame

Project clone Flappy Bird bằng Unity 6 với gameplay 2D: nhấn để bay, né ống, tính điểm, game over khi va chạm chướng ngại vật.

## Tổng quan

- Engine: Unity `6000.3.12f1`
- Scene chính: `Assets/Scenes/MainScenes.unity`
- Render pipeline: URP (`com.unity.render-pipelines.universal`)
- Input system: Unity Input System (`com.unity.inputsystem`)
- Kiểu dự án: single-scene arcade game (2D)

## Chạy dự án

1. Mở Unity Hub.
2. Add/Open folder project: `D:\Code\Unity\FalppyBirdGame`.
3. Mở scene `Assets/Scenes/MainScenes.unity`.
4. Nhấn **Play** trong Unity Editor.

Lưu ý: Input trong project đang map cho chuột trái và touch (xem mục Ánh xạ đầu vào).

## Ánh xạ đầu vào (Input Mapping)

File: `Assets/FlappyBird/PlayerInputAction.inputactions`

Action map `GamePlay` có action `Tap` (Button):
- `<Mouse>/leftButton`
- `<Touchscreen>/Press`

`Tap` được xử lý bởi `PlayerController.OnTap(...)`.

## Vòng đời gameplay (state machine)

Enum state: `Assets/FlappyBird/Scripts/GameState.cs`

- `StartScreen`: hiện logo + nút Start
- `GameReady`: reset player/spawner, chờ người chơi tap lần đầu
- `Playing`: bắt đầu rơi/trong lúc chơi, spawn pipe, cộng điểm
- `GameOver`: dừng chơi, rung camera, hiện panel game over

State được điều phối bởi `GameManager`.

## Các script chính

### `GameManager.cs`
Script trung tâm điều phối game:
- Quản lý singleton `GameManager.Instance`
- Chuyển state (`StartGame()`, `GamePlay()`, `GameOver()`)
- Quản lý điểm hiện tại và best score (PlayerPrefs key `BestScore`)
- Điều khiển UI panel (start, ready, game over)
- Gọi reset qua `PlayerController.ResetPlayer()` và `PipeSpawner.ResetSpawner()`
- Phát âm thanh cộng điểm qua `AudioManager`
- Hiệu ứng shake camera khi game over

### `PlayerController.cs`
Điều khiển nhân vật:
- Idle floating ở `StartScreen`/`GameReady`
- Nhận input `Tap` để jump (`AddForce`)
- Tự động vào state `Playing` ở lần tap đầu trong `GameReady`
- Xử lý va chạm `Obstacle` -> âm thanh hit/die + `GameOver()`
- `ResetPlayer()` đưa player về vị trí/rotation/velocity ban đầu

### `PipeSpawner.cs`
Spawn pipe theo pool:
- Đặt vị trí spawn ở bên phải màn hình (`SetSpawnPositon()`)
- Tạo pool pipe một lần (`CreatePool()`)
- Spawn theo chu kỳ `spawnRate` khi state là `Playing`
- Random độ cao theo `heightOffset`
- Thu hồi pipe về pool qua `ReturnPipeToPool(...)`
- Reset toàn bộ pool qua `ResetSpawner()`

### `PipeController.cs`
Điều khiển từng cụm pipe:
- Di chuyển sang trái theo `moveSpeed`
- Khi ra khỏi màn (`hideXPosition`) thì trả về pool
- Mỗi lần enable sẽ reset score zone để tránh cộng điểm lặp

### `ScoreZone.cs`
Tính điểm khi player đi qua:
- Trigger với object tag `Player`
- Mỗi pipe chỉ cộng 1 lần nhờ có `_hasScored`
- Gọi `GameManager.Instance.AddScore()`

### `GroundScroller.cs`
Tạo hiệu ứng nền đất chạy:
- Dịch `mainTextureOffset` theo `scrollSpeed`
- Dừng scroll khi `GameOver`

### `CanvasConatroller.cs`
Căn chỉnh UI scaler theo aspect ratio camera:
- Tính toán `matchWidthOrHeight` cho màn hình rộng/hẹp

### `AudioManager.cs`
Quản lý âm thanh dạng singleton:
- Các clip: `fly`, `score`, `hit`, `die`
- Play one-shot thông qua `AudioSource`

## Cấu trúc thư mục liên quan

- `Assets/Scenes/`
  - `MainScenes.unity`: scene gameplay chính
- `Assets/FlappyBird/Scripts/`
  - Toàn bộ script gameplay/UI/audio
- `Assets/FlappyBird/Prefab/`
  - Prefab player, pipe, UI object (nếu được gắn trong scene)
- `Assets/FlappyBird/Sounds/`
  - Audio clips cho fly/score/hit/die
- `Assets/FlappyBird/Animations/`
  - Animation/Animator cho nhân vật
- `Assets/FlappyBird/MyUI/`
  - Tài nguyên UI trong game

## Dependencies quan trọng

Tham chiếu từ `Packages/manifest.json`:
- `com.unity.inputsystem`
- `com.unity.render-pipelines.universal`
- `com.unity.ugui`
- `com.unity.2d.*` (bộ công cụ và runtime 2D)

## Ghi chú cho dev

- Project dùng object pooling cho pipe để tránh instantiate/destroy liên tục.
- Điểm cao nhất được lưu local bằng `PlayerPrefs`.
- Nếu thêm scene mới, cần cập nhật flow state hoặc bổ sung scene loader phù hợp.
- Kiểm tra tag `Player` và `Obstacle` trong scene/prefab để collision và score chạy đúng.

## Hướng mở rộng đề xuất

- Thêm pausing và resume theo state riêng.
- Thêm menu settings (âm thanh, độ khó).
- Thêm hệ thống lưu profile/leaderboard.
- Thêm play mode tests cho logic score và state transition.
