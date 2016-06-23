#include "mainwindow.h"
#include "ui_mainwindow.h"

MainWindow::MainWindow(QWidget *parent) :
    QMainWindow(parent),
    ui(new Ui::MainWindow)
{
    ui->setupUi(this);

    connect(ui->applyButton, SIGNAL(released()), this, SLOT(applyChanges()));

    QXmlStreamReader Rxml;

        QString filename = "parametres.xml";

        QFile file(filename);
        if (file.open(QFile::ReadOnly | QFile::Text))
        {
            qDebug()<<"toto";
            Rxml.setDevice(&file);
            Rxml.readNext();

            while(!Rxml.atEnd()){
                if(Rxml.isStartElement()){
                    if(Rxml.name() == "networkListeningPort")
                    {
                        while(!Rxml.atEnd()){
                            if(Rxml.isEndElement()){
                                Rxml.readNext();
                                break;
                            }
                            else if(Rxml.isStartElement()){
                                 if(Rxml.name() == "Value"){
                                    ui->networkListeningPort->setText(Rxml.readElementText());
                                 }
                                 Rxml.readNext();
                            }
                            else{
                                Rxml.readNext();
                            }
                        }
                    }
                    else if(Rxml.name() == "networkSendingPort")
                    {
                        while(!Rxml.atEnd()){
                            if(Rxml.isEndElement()){
                                Rxml.readNext();
                                break;
                            }
                            else if(Rxml.isStartElement()){
                                 if(Rxml.name() == "Value"){
                                    ui->networkSendingPort->setText(Rxml.readElementText());
                                 }
                                 Rxml.readNext();
                            }
                            else{
                                Rxml.readNext();
                            }
                        }
                    }
                    else if(Rxml.name() == "networkServerIP")
                    {
                        while(!Rxml.atEnd()){
                            if(Rxml.isEndElement()){
                                Rxml.readNext();
                                break;
                            }
                            else if(Rxml.isStartElement()){
                                 if(Rxml.name() == "Value"){
                                     ui->networkServerIP->setText(Rxml.readElementText());
                                 }
                                 Rxml.readNext();
                            }
                            else{
                                Rxml.readNext();
                            }
                        }
                    }
                    else if(Rxml.name() == "pirateBoatsInitInterval")
                    {
                        while(!Rxml.atEnd()){
                            if(Rxml.isEndElement()){
                                Rxml.readNext();
                                break;
                            }
                            else if(Rxml.isStartElement()){
                                 if(Rxml.name() == "Value"){
                                     ui->pirateBoatsInitInterval->setText(Rxml.readElementText());
                                 }
                                 Rxml.readNext();
                            }
                            else{
                                Rxml.readNext();
                            }
                        }
                    }
                    else if(Rxml.name() == "pirateBoatsIncreaseRate")
                    {
                        while(!Rxml.atEnd()){
                            if(Rxml.isEndElement()){
                                Rxml.readNext();
                                break;
                            }
                            else if(Rxml.isStartElement()){
                                 if(Rxml.name() == "Value"){
                                     ui->pirateBoatsIncreaseRate->setText(Rxml.readElementText());
                                 }
                                 Rxml.readNext();
                            }
                            else{
                                Rxml.readNext();
                            }
                        }
                    }
                    else{
                        Rxml.readNext();
                    }
                }
                else{
                    Rxml.readNext();
                }
            }

            file.close();

            if (Rxml.hasError()){
               std::cerr << "Error: Failed to parse file "
                         << qPrintable(filename) << ": "
                         << qPrintable(Rxml.errorString()) << std::endl;
            }
            else if (file.error() != QFile::NoError){
                std::cerr << "Error: Cannot read file " << qPrintable(filename)
                          << ": " << qPrintable(file.errorString())
                          << std::endl;
            }
        }
}


void MainWindow::applyChanges() {

    if(verify()){
        QString filename = "parametres.xml";
        QFile file(filename);
        file.open(QIODevice::WriteOnly);

        QXmlStreamWriter xmlWriter(&file);
        xmlWriter.setAutoFormatting(true);
        xmlWriter.writeStartDocument();

        xmlWriter.writeStartElement("Archipel");

        xmlWriter.writeStartElement("networkListeningPort");
        xmlWriter.writeTextElement("Value", ui->networkListeningPort->text());
        xmlWriter.writeEndElement();

        xmlWriter.writeStartElement("networkSendingPort");
        xmlWriter.writeTextElement("Value", ui->networkSendingPort->text());
        xmlWriter.writeEndElement();

        xmlWriter.writeStartElement("networkServerIP");
        xmlWriter.writeTextElement("Value", ui->networkServerIP->text());
        xmlWriter.writeEndElement();

        xmlWriter.writeStartElement("pirateBoatsInitInterval");
        xmlWriter.writeTextElement("Value", ui->pirateBoatsInitInterval->text());
        xmlWriter.writeEndElement();

        xmlWriter.writeStartElement("pirateBoatsIncreaseRate");
        xmlWriter.writeTextElement("Value", ui->pirateBoatsIncreaseRate->text());
        xmlWriter.writeEndElement();

        xmlWriter.writeEndElement();

        file.close();
    }
}

bool MainWindow::verify() {
    QRegExp rxIP("[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}");
    QRegExpValidator regValidatorIP( rxIP, 0 );

    bool v = true;

    if(ui->networkListeningPort->text().toInt() && ui->networkListeningPort->text().toInt()<65000){
        ui->networkListeningPort->setStyleSheet("QLineEdit { background: rgb(0, 255, 0); }");
    }
    else{
        ui->networkListeningPort->setStyleSheet("QLineEdit { background: rgb(255, 0, 0); }");
        v=false;
    }

    if(ui->networkSendingPort->text().toInt() && ui->networkSendingPort->text().toInt()<65000){
        ui->networkSendingPort->setStyleSheet("QLineEdit { background: rgb(0, 255, 0); }");
    }
    else{
        ui->networkSendingPort->setStyleSheet("QLineEdit { background: rgb(255, 0, 0); }");
        v=false;
    }

    QString t = ui->networkServerIP->text();
    int pos = 0;

    if(regValidatorIP.validate(t,pos)==QRegExpValidator::Acceptable){
        ui->networkServerIP->setStyleSheet("QLineEdit { background: rgb(0, 255, 0); }");
    }
    else{
        ui->networkServerIP->setStyleSheet("QLineEdit { background: rgb(255, 0, 0); }");
        v=false;
    }

    if(ui->pirateBoatsInitInterval->text().toInt() && ui->pirateBoatsInitInterval->text().toInt()<65000){
        ui->pirateBoatsInitInterval->setStyleSheet("QLineEdit { background: rgb(0, 255, 0); }");
    }
    else{
        ui->pirateBoatsInitInterval->setStyleSheet("QLineEdit { background: rgb(255, 0, 0); }");
        v=false;
    }

    if(ui->pirateBoatsIncreaseRate->text().toDouble() && ui->pirateBoatsIncreaseRate->text().toDouble()<=1){
        ui->pirateBoatsIncreaseRate->setStyleSheet("QLineEdit { background: rgb(0, 255, 0); }");
    }
    else{
        ui->pirateBoatsIncreaseRate->setStyleSheet("QLineEdit { background: rgb(255, 0, 0); }");
        v=false;
    }

    return v;
}
