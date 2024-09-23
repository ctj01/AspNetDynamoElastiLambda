import os
import json
from elasticsearch import Elasticsearch, helpers

# Conexión a Elasticsearch
es = Elasticsearch(
    ['http://localhost:9200'],  # Cambia el endpoint si es necesario
    http_auth=('elastic', '010203')  # Agrega las credenciales de autenticación
)


# Directorio donde están los archivos batch
batch_dir = './'

# Obtener todos los archivos batch en el directorio
batch_files = [f for f in os.listdir(batch_dir) if f.startswith('movies_batch_') and f.endswith('.json')]

# Iterar sobre todos los archivos y ejecutar la inserción en Elasticsearch
for batch_file in sorted(batch_files):
    file_path = os.path.join(batch_dir, batch_file)
    print(f'Inserting batch: {batch_file}')
    
    # Leer el archivo JSON
    with open(file_path, 'r') as f:
        data = json.load(f)  # Cargar todo el archivo JSON

    # Extraer y convertir las películas desde la estructura de DynamoDB a formato Elasticsearch
    movies = [
        {
            "id": item["PutRequest"]["Item"]["id"]["S"],  # Extraer valor de "S"
            "title": item["PutRequest"]["Item"]["title"]["S"],  # Extraer valor de "S"
            "genres": item["PutRequest"]["Item"]["genres"]["S"]  # Extraer valor de "S"
        }
        for item in data["movies"]
    ]
    
    # Crear las acciones para el bulk
    actions = [
        {
            "_index": "movies",  # Nombre del índice en Elasticsearch
            "_id": movie["id"],  # Asignar el campo "id" como el identificador del documento
            "_source": movie     # Los datos del documento
        }
        for movie in movies
    ]
    
    # Insertar el batch en Elasticsearch
    try:
        helpers.bulk(es, actions)
        print(f'Batch {batch_file} inserted successfully.')
    except Exception as e:
        print(f'Error inserting batch {batch_file}: {e}')
        break

print("All batches processed.")
