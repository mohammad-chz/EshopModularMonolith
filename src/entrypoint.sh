#!/bin/bash
/opt/mssql/bin/sqlservr &
SQL_PID=$!

echo "Waiting for SQL Server to be fully ready..."

for i in {1..90}; do
    # Strictly check for exactly "1" as the output line
    RESULT=$(/opt/mssql-tools18/bin/sqlcmd \
        -S localhost \
        -U SA \
        -P 'P@ssw0rd123!' \
        -No \
        -Q "SET NOCOUNT ON; SELECT 1 AS ready" \
        -h -1 2>/dev/null | tr -d ' \r\n')

    if [ "$RESULT" = "1" ]; then
        echo "SQL Server is fully ready after $((i * 2)) seconds."
        break
    fi

    echo "Attempt $i: not ready yet, retrying in 2s..."
    sleep 2

    if [ $i -eq 90 ]; then
        echo "ERROR: SQL Server did not become ready in time."
        exit 1
    fi
done

echo "Creating eshopdb if not exists..."
/opt/mssql-tools18/bin/sqlcmd \
    -S localhost \
    -U SA \
    -P 'P@ssw0rd123!' \
    -No \
    -Q "IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'eshopdb') BEGIN CREATE DATABASE eshopdb; PRINT 'eshopdb created.'; END ELSE PRINT 'eshopdb already exists.';"

if [ $? -eq 0 ]; then
    echo "Done."
else
    echo "ERROR: Failed to create database."
fi

wait $SQL_PID