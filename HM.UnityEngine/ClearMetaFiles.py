import os
import sys
from typing import List


def GetFiles(path=None) -> List[str]:
    result = []
    if path is None:
        path = os.getcwd()
    for i in os.listdir(path):
        rFullName = path+"/"+i
        if os.path.isfile(rFullName):
            result.append(rFullName)
    return result


def GetDirectories(path=None) -> List[str]:
    result = []
    if path is None:
        path = os.getcwd()
    for i in os.listdir(path):
        rFullName = path+"/"+i
        if os.path.isdir(rFullName):
            result.append(rFullName)
    return result


def GetFilesRescursly(path=None) -> List[str]:
    result = []
    for i in GetDirectories(path):
        result += GetFilesRescursly(i)

    for i in GetFiles(path):
        result.append(i)
    return result


metaFiles = [i for i in GetFilesRescursly() if i.endswith(".meta")]
print("\n".join(metaFiles))
n = input("(y/n)")
if n == "y":
    for file in metaFiles:
        os.remove(file)
