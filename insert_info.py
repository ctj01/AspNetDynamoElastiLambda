import os
import subprocess

# Directorio donde están los archivos batch
batch_dir = './movies'

# Obtener todos los archivos batch en el directorio
batch_files = [f for f in os.listdir(batch_dir) if f.startswith('movies_batch_') and f.endswith('.json')]

# Iterar sobre todos los archivos y ejecutar batch-write-item
for batch_file in sorted(batch_files):
    file_path = os.path.join(batch_dir, batch_file)
    print(f'Inserting batch: {batch_file}')
    
    # Ejecutar el comando de AWS CLI para insertar los datos
    result = subprocess.run([
        'aws', 'dynamodb', 'batch-write-item',
        '--request-items', f'file://{file_path}',
        '--endpoint-url', 'http://localhost:8000'
    ])
    
    # Verificar si hubo algún error
    if result.returncode != 0:
        print(f'Error inserting batch {batch_file}. Stopping.')
        break

print("All batches processed.")
