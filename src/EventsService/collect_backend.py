import os

# --- НАСТРОЙКИ ---
# Имя выходного файла
output_file = 'events_service_full.txt'

# Какие расширения файлов собирать
# Добавил .cs (код C#), .csproj (проект), .json (конфиги), .sql (если есть миграции)
extensions_to_include = {'.cs', '.csproj', '.json', '.xml', '.sql', '.dockerfile'}

# Точные имена файлов без расширений (например, Dockerfile)
specific_files = {'Dockerfile', 'docker-compose.yml', 'Makefile'}

# Папки, которые НУЖНО ИГНОРИРОВАТЬ (системные и сборочные)
dirs_to_ignore = {
    'bin', 'obj',           # C# build folders
    '.git', '.vs', '.idea', # IDE and Git
    'TestResults',          # Tests
    'node_modules',         # Если вдруг есть JS
    'migrations'            # Можно убрать, если миграции нужны
}

# Файлы, которые игнорируем
files_to_ignore = {output_file, 'appsettings.Development.json'} # Можно исключить секреты

def collect_files():
    # Получаем текущую директорию, где лежит скрипт
    current_dir = os.getcwd()
    print(f"Сбор файлов из: {current_dir}")

    with open(output_file, 'w', encoding='utf-8') as outfile:
        for root, dirs, files in os.walk(current_dir):
            # 1. Фильтрация папок (изменяем список dirs на месте)
            dirs[:] = [d for d in dirs if d not in dirs_to_ignore]
            
            for file in files:
                if file in files_to_ignore:
                    continue

                # Получаем расширение файла
                ext = os.path.splitext(file)[1].lower()
                
                # Проверяем, подходит ли файл
                if ext in extensions_to_include or file in specific_files:
                    file_path = os.path.join(root, file)
                    
                    # Делаем красивый относительный путь для заголовка
                    relative_path = os.path.relpath(file_path, current_dir)

                    try:
                        with open(file_path, 'r', encoding='utf-8') as infile:
                            content = infile.read()
                            
                            # Записываем красиво
                            outfile.write(f"{'='*60}\n")
                            outfile.write(f"FILE: {relative_path}\n")
                            outfile.write(f"{'='*60}\n")
                            outfile.write(content)
                            outfile.write("\n\n")
                            
                        print(f"Добавлен: {relative_path}")
                    except Exception as e:
                        print(f"Ошибка чтения {relative_path}: {e}")

    print(f"\nГотово! Файл сохранен как: {output_file}")

if __name__ == '__main__':
    collect_files()