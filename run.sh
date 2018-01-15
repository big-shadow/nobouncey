#!/bin/sh

# dot net NoBouncey.dll ./input.csv ./Output/output.csv

split -l 500 ./Output/output.csv ./Output/Splits/s

for file in ./Output/Splits/*; do
    cat ./headers.txt $file >> $file.$$
    mv $file.$$ $file
done