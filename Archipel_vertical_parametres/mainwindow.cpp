#include "mainwindow.h"
#include "ui_mainwindow.h"

MainWindow::MainWindow(QWidget *parent) :
    QMainWindow(parent),
    ui(new Ui::MainWindow)
{
    ui->setupUi(this);

    connect(ui->clearButton, SIGNAL(released()), this, SLOT(clearScores()));
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
                    else if(Rxml.name() == "pirateBoatsStart")
                    {
                        while(!Rxml.atEnd()){
                            if(Rxml.isEndElement()){
                                Rxml.readNext();
                                break;
                            }
                            else if(Rxml.isStartElement()){
                                 if(Rxml.name() == "Value"){
                                     ui->pirateBoatsStart->setText(Rxml.readElementText());
                                 }
                                 Rxml.readNext();
                            }
                            else{
                                Rxml.readNext();
                            }
                        }
                    }
                    else if(Rxml.name() == "pirateBoatsIncreaseTime")
                    {
                        while(!Rxml.atEnd()){
                            if(Rxml.isEndElement()){
                                Rxml.readNext();
                                break;
                            }
                            else if(Rxml.isStartElement()){
                                 if(Rxml.name() == "Value"){
                                     ui->pirateBoatsIncreaseTime->setText(Rxml.readElementText());
                                 }
                                 Rxml.readNext();
                            }
                            else{
                                Rxml.readNext();
                            }
                        }
                    }
                    else if(Rxml.name() == "timerGame")
                    {
                        while(!Rxml.atEnd()){
                            if(Rxml.isEndElement()){
                                Rxml.readNext();
                                break;
                            }
                            else if(Rxml.isStartElement()){
                                 if(Rxml.name() == "Value"){
                                     ui->timerGame->setText(Rxml.readElementText());
                                 }
                                 Rxml.readNext();
                            }
                            else{
                                Rxml.readNext();
                            }
                        }
                    }
                    else if(Rxml.name() == "timerChallenge")
                    {
                        while(!Rxml.atEnd()){
                            if(Rxml.isEndElement()){
                                Rxml.readNext();
                                break;
                            }
                            else if(Rxml.isStartElement()){
                                 if(Rxml.name() == "Value"){
                                     ui->timerChallenge->setText(Rxml.readElementText());
                                 }
                                 Rxml.readNext();
                            }
                            else{
                                Rxml.readNext();
                            }
                        }
                    }
                    else if(Rxml.name() == "timerDisturbance")
                    {
                        while(!Rxml.atEnd()){
                            if(Rxml.isEndElement()){
                                Rxml.readNext();
                                break;
                            }
                            else if(Rxml.isStartElement()){
                                 if(Rxml.name() == "Value"){
                                     ui->timerDisturbance->setText(Rxml.readElementText());
                                 }
                                 Rxml.readNext();
                            }
                            else{
                                Rxml.readNext();
                            }
                        }
                    }
                    else if(Rxml.name() == "fileScores")
                    {
                        while(!Rxml.atEnd()){
                            if(Rxml.isEndElement()){
                                Rxml.readNext();
                                break;
                            }
                            else if(Rxml.isStartElement()){
                                 if(Rxml.name() == "Value"){
                                     ui->fileScores->setText(Rxml.readElementText());
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

void MainWindow::applyChanges()
{
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

        xmlWriter.writeStartElement("pirateBoatsStart");
        xmlWriter.writeTextElement("Value", ui->pirateBoatsStart->text());
        xmlWriter.writeEndElement();

        xmlWriter.writeStartElement("pirateBoatsIncreaseRate");
        xmlWriter.writeTextElement("Value", ui->pirateBoatsIncreaseTime->text());
        xmlWriter.writeEndElement();

        xmlWriter.writeStartElement("timerGame");
        xmlWriter.writeTextElement("Value", ui->timerGame->text());
        xmlWriter.writeEndElement();

        xmlWriter.writeStartElement("timerChallenge");
        xmlWriter.writeTextElement("Value", ui->timerChallenge->text());
        xmlWriter.writeEndElement();

        xmlWriter.writeStartElement("timerDisturbance");
        xmlWriter.writeTextElement("Value", ui->timerDisturbance->text());
        xmlWriter.writeEndElement();

        xmlWriter.writeStartElement("fileScores");
        xmlWriter.writeTextElement("Value", ui->fileScores->text());
        xmlWriter.writeEndElement();

        xmlWriter.writeEndElement();

        file.close();
    }
}

void MainWindow::clearScores() {
    qDebug()<<"lel";

    QMessageBox messageBox(QMessageBox::Question, tr("Erase scores"),
                               tr("Are you sure you want to erase all the stored scores ?"),
                               QMessageBox::Yes | QMessageBox::No);
        int ret = messageBox.exec();

    QString filename = "scores.txt";
    QFile file(filename);
    file.open(QFile::WriteOnly|QFile::Truncate);

    file.close();
}

bool MainWindow::verify() {
    QRegExp rxIP("[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}");
    QRegExpValidator regValidatorIP( rxIP, 0 );

    QRegExp rxFile("[a-zA-Z]+\.txt$");
    QRegExpValidator regValidatorFile( rxFile, 0 );

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

    if(ui->pirateBoatsStart->text().toInt() && ui->pirateBoatsStart->text().toInt()<65000){
        ui->pirateBoatsStart->setStyleSheet("QLineEdit { background: rgb(0, 255, 0); }");
    }
    else{
        ui->pirateBoatsStart->setStyleSheet("QLineEdit { background: rgb(255, 0, 0); }");
        v=false;
    }

    if(ui->pirateBoatsIncreaseTime->text().toInt() && ui->pirateBoatsIncreaseTime->text().toInt()<65000){
        ui->pirateBoatsIncreaseTime->setStyleSheet("QLineEdit { background: rgb(0, 255, 0); }");
    }
    else{
        ui->pirateBoatsIncreaseTime->setStyleSheet("QLineEdit { background: rgb(255, 0, 0); }");
        v=false;
    }

    if(ui->timerGame->text().toInt() && ui->timerGame->text().toInt()<65000){
        ui->timerGame->setStyleSheet("QLineEdit { background: rgb(0, 255, 0); }");
    }
    else{
        ui->timerGame->setStyleSheet("QLineEdit { background: rgb(255, 0, 0); }");
        v=false;
    }

    if(ui->timerChallenge->text().toInt() && ui->timerChallenge->text().toInt()<65000){
        ui->timerChallenge->setStyleSheet("QLineEdit { background: rgb(0, 255, 0); }");
    }
    else{
        ui->timerChallenge->setStyleSheet("QLineEdit { background: rgb(255, 0, 0); }");
        v=false;
    }
    if(ui->timerDisturbance->text().toInt() && ui->timerDisturbance->text().toInt()<65000){
        ui->timerDisturbance->setStyleSheet("QLineEdit { background: rgb(0, 255, 0); }");
    }
    else{
        ui->timerDisturbance->setStyleSheet("QLineEdit { background: rgb(255, 0, 0); }");
        v=false;
    }

    t = ui->fileScores->text();

    if(regValidatorFile.validate(t,pos)==QRegExpValidator::Acceptable){
        ui->fileScores->setStyleSheet("QLineEdit { background: rgb(0, 255, 0); }");
    }
    else{
        ui->fileScores->setStyleSheet("QLineEdit { background: rgb(255, 0, 0); }");
        v=false;
    }

    return v;
}

