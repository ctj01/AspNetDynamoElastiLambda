import json

# Cargar los datos desde movies.json
with open('movies.json', 'r') as f:
    data = json.load(f)

# Nombre de la tabla
table_name = "movies"

# Dividir en lotes de 25 ítems
batch_size = 25
for i in range(0, len(data[table_name]), batch_size):
    batch = {table_name: data[table_name][i:i + batch_size]}
    with open(f'movies_batch_{i // batch_size}.json', 'w') as f_batch:
        json.dump(batch, f_batch, indent=4)
    print(f'Created batch file: movies_batch_{i // batch_size}.json')
