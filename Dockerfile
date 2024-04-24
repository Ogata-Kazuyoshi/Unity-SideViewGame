# ベースイメージ
FROM ubuntu:latest

# 必要なパッケージのインストール
RUN apt-get update && apt-get install -y \
    libglu1 \
    xorg-dev \
    && rm -rf /var/lib/apt/lists/*

# アプリケーションのコピー
# アプリケーションとデータフォルダのコピー
COPY ./MyUnityApp.x86_64 /usr/src/myapp/MyUnityApp.x86_64
# COPY ./MyUnityApp_Data /usr/src/myapp/MyUnityApp_Data

# ワーキングディレクトリの設定
WORKDIR /usr/src/myapp

# アプリケーションの実行
# CMD ["./MyUnityApp"]
CMD ["./MyUnityApp.x86_64"]