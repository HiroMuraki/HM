using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Net.Http.Headers;
using System.Net.NetworkInformation;
using System.Reflection.Emit;
using System.Security.Cryptography;
using System;
using System.Threading;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Net.Mail;
using System.Xml;

namespace HM.Data.DataCenters;

public interface ISingleDataCenter<T>
{
    T? Get();

    int Update(T item);

    int Delete();
}