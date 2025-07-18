# 🎬 YtDlpGUI - Графическая оболочка для yt-dlp


[![GitHub release](https://img.shields.io/github/v/release/wulkan-Git/YtDlpGUI)](https://github.com/wulkan-Git/YtDlpGUI/releases)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
![Platform](https://img.shields.io/badge/Platform-Windows-blue)
[![Downloads](https://img.shields.io/github/downloads/wulkan-Git/YtDlpGUI/total)](https://github.com/wulkan-Git/YtDlpGUI/releases)

**YtDlpGUI** - это удобная графическая оболочка для популярной консольной утилиты `yt-dlp.exe`, позволяющая скачивать видео и аудио с различных интернет-ресурсов без использования командной строки.

![YtDlpGUI Interface]([screenshot.png](https://private-user-images.githubusercontent.com/208951367/465761578-d3ae4842-1e08-4808-8b56-ed59f78bc474.png?jwt=eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJnaXRodWIuY29tIiwiYXVkIjoicmF3LmdpdGh1YnVzZXJjb250ZW50LmNvbSIsImtleSI6ImtleTUiLCJleHAiOjE3NTI4NDc3MzksIm5iZiI6MTc1Mjg0NzQzOSwicGF0aCI6Ii8yMDg5NTEzNjcvNDY1NzYxNTc4LWQzYWU0ODQyLTFlMDgtNDgwOC04YjU2LWVkNTlmNzhiYzQ3NC5wbmc_WC1BbXotQWxnb3JpdGhtPUFXUzQtSE1BQy1TSEEyNTYmWC1BbXotQ3JlZGVudGlhbD1BS0lBVkNPRFlMU0E1M1BRSzRaQSUyRjIwMjUwNzE4JTJGdXMtZWFzdC0xJTJGczMlMkZhd3M0X3JlcXVlc3QmWC1BbXotRGF0ZT0yMDI1MDcxOFQxNDAzNTlaJlgtQW16LUV4cGlyZXM9MzAwJlgtQW16LVNpZ25hdHVyZT00YzRhN2U5ODMyYmIzM2UxNGE3NTNhMWVhYzBmNGU2YmFiOTZmOTE5N2U0NTM4OTRhZTVhZGM1ZDQ1NjEwNjAyJlgtQW16LVNpZ25lZEhlYWRlcnM9aG9zdCJ9.PBOhA1L2HrJAy7VBdQTeQ_hjBMsqpEo2zhtuNXFf8pc)) <!-- Замените на реальный путь к скриншоту -->

## 🌟 Основные возможности

- 📥 **Скачивание видео/плейлистов** по прямой ссылке
- 📁 **Указание папки для сохранения** с помощью диалога выбора
- 🔊 **Извлечение аудиодорожки** из видеофайлов
- 🏆 **Скачивание в лучшем качестве** с автоматическим объединением видео и аудио
- 📊 **Просмотр доступных форматов** для конкретного видео
- 📚 **Пакетное скачивание** из файла со списком URL
- 🎨 **Цветной вывод логов**:
  - Стандартный вывод - черный
  - Ошибки - красный
  - Успех - зеленый

## ⚙️ Особенности реализации

- **Полностью асинхронная работа** без блокировки интерфейса
- **Автоматическая проверка** наличия yt-dlp.exe при запуске
- **Поддержка UTF-8** для корректного отображения вывода
- **Автоматическая прокрутка логов** к новым сообщениям
- **Интегрированные ссылки** на GitHub yt-dlp и сайт автора
- **Обработка ошибок** с выводом в лог программы

## 🚀 Как использовать

1. Скачайте последнюю версию программы из раздела [Releases](https://github.com/wulkan-Git/YtDlpGUI/releases)
2. Распакуйте архив в любую папку
3. Запустите `YtDlpGUI.exe`
4. Вставьте ссылку на видео или плейлист в поле "URL"
5. Выберите параметры скачивания:
   - Формат (видео/аудио)
   - Качество
   - Папку для сохранения
6. Нажмите "Старт" для начала загрузки

## 📦 Требования

- **Операционная система**: Windows 7 или новее
- **.NET Framework**: 4.7.2 или выше
- **yt-dlp**: Автоматически скачивается при первом запуске

## 🔧 Для разработчиков

```bash
# Клонировать репозиторий
git clone https://github.com/wulkan-Git/YtDlpGUI.git

# Открыть решение в Visual Studio
YtDlpGUI.sln
