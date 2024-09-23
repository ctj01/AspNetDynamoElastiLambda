const { DynamoDBClient } = require('@aws-sdk/client-dynamodb');
const { Client } = require('@elastic/elasticsearch');
const { unmarshall } = require('@aws-sdk/util-dynamodb');

// Crear un cliente de DynamoDB
const dynamodb = new DynamoDBClient({ region: 'us-east-1' });

// Configurar cliente de Elasticsearch
const esClient = new Client({ 
    node: 'http://elasticsearch:9200', 
    auth: { username: 'elastic', password: '010203' } 
});

exports.handler = async (event) => {
    for (const record of event.Records) {
        const eventName = record.eventName;  // INSERT, MODIFY, REMOVE
        const newImage =  unmarshall(record.dynamodb.NewImage);  // Cambia de AWS.DynamoDB.Converter.unmarshall

        if (eventName === 'INSERT' || eventName === 'MODIFY') {
            // Insertar o actualizar en Elasticsearch
            await esClient.index({
                index: 'movies',
                id: newImage.id,  // ID único de DynamoDB
                body: newImage     // Datos completos del ítem
            });
        } else if (eventName === 'REMOVE') {
            // Eliminar el documento de Elasticsearch
            await esClient.delete({
                index: 'movies',
                id: record.dynamodb.Keys.id.S
            });
        }
    }
};
