import requests
import json
import hashlib  # To generate a unique ID by hashing
from azure.cosmos import CosmosClient
from dotenv import load_dotenv
import os
from datetime import datetime, timedelta

# Load environment variables from .env file
load_dotenv()

# Initialize Cosmos DB client with credentials from environment variables
cosmos_url = os.getenv("COSMOS_DB_URL")  # Cosmos DB account URL
cosmos_key = os.getenv("COSMOS_DB_KEY")   # Cosmos DB account key
database_name = os.getenv("COSMOS_DB_NAME")  # Database name
container_name = os.getenv("COSMOS_DB_CONTAINER_NAME")  # Container name
newsapi_key = os.getenv("NEWSAPI_KEY")  # NewsAPI Key

# Create a Cosmos DB client
cosmos_client = CosmosClient(cosmos_url, cosmos_key)

# Get a reference to the database and container
database = cosmos_client.get_database_client(database_name)
container = database.get_container_client(container_name)

def generate_id(url):
    # Generate a unique ID by hashing the URL
    return hashlib.md5(url.encode('utf-8')).hexdigest()

def fetch_and_store_stock_news():
    # Load tickers from JSON file
    with open('ticker.json', 'r') as f:
        tickers = json.load(f)

    # Calculate the date range for the last 5 days
    to_date = datetime.now()
    from_date = to_date - timedelta(days=5)

    # Format the dates to the required string format (YYYY-MM-DD)
    to_date_str = to_date.strftime('%Y-%m-%d')
    from_date_str = from_date.strftime('%Y-%m-%d')

    # Loop through each ticker and fetch news
    for ticker_info in tickers:
        ticker = ticker_info['ticker']  # Extract the ticker symbol
        try:
            # Construct the API URL for the NewsAPI
            url = (
                f'https://newsapi.org/v2/everything?'
                f'q={ticker}&'
                f'from={from_date_str}&'
                f'sortBy=popularity&'
                f'apiKey={newsapi_key}'
            )

            # Make the request to the NewsAPI
            response = requests.get(url)
            data = response.json()

            if data.get("status") == "ok":
                articles = data.get("articles", [])

                # Prepare the data for Cosmos DB
                for article in articles:
                    # Generate a unique ID using the hashed URL
                    article_id = generate_id(article.get('url'))

                    article_data = {
                        'id': article_id,  # Using the generated hash ID
                        'ticker': ticker,
                        'title': article.get('title'),
                        'description': article.get('description'),
                        'url': article.get('url'),
                        'content': article.get('content'),
                        'published_at': article.get('publishedAt'),
                    }

                    # Store the news article in Cosmos DB
                    container.upsert_item(article_data)

                print(f'Stored news for ticker: {ticker}')
            else:
                print(f"Failed to fetch news for ticker {ticker}. Status: {data.get('status')}")

        except Exception as e:
            print(f'Error fetching news for ticker {ticker}: {e}')

if __name__ == "__main__":
    fetch_and_store_stock_news()
