# ベースイメージ
FROM ubuntu:latest

# 必要なパッケージのインストール
RUN apt-get update && apt-get install -y \
    libglu1 \
    xorg-dev \
    && rm -rf /var/lib/apt/lists/*

# アプリケーションとデータフォルダのコピー
COPY ./MyUnityApp.x86_64 /usr/src/myapp/MyUnityApp.x86_64
COPY ./MyUnityApp_Data /usr/src/myapp/MyUnityApp_Data

# 環境変数の設定
ENV LD_LIBRARY_PATH=/usr/src/myapp/MyUnityApp_Data:$LD_LIBRARY_PATH

# ワーキングディレクトリの設定
WORKDIR /usr/src/myapp

# アプリケーションの実行
CMD ["./MyUnityApp.x86_64"]